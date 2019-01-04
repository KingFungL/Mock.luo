using Autofac;
using AutoMapper;
using Mock.Code.Attribute;
using Mock.Code.Helper;
using Mock.Code.Json;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Data.Repository;
using Mock.Domain.Interface;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class ArticleController : CrudController<Article, ArticleViewModel>
    {
        // GET: Plat/Article

        private readonly IArticleRepository _articleRepository;
        private readonly IItemsDetailRepository _itemsDetailRepository;
        public ArticleController(IArticleRepository articleRepository, IItemsDetailRepository itemsDetailRepository, IComponentContext container) : base(container)
        {
            this._articleRepository = articleRepository;
            this._itemsDetailRepository = itemsDetailRepository;
        }

        public override ActionResult Form(int id)
        {
            //取出文章对应的多个标签Id
            var tagActive = _articleRepository.Queryable(u => u.DeleteMark == false && u.Id == id)
                .Select(u => u.TagArts.Select(r => r.TagId)).AsEnumerable().FirstOrDefault();

            ViewBag.TagActive = JsonHelper.SerializeObject(tagActive);

            return base.Form(id);
        }

        public ActionResult TitleContent(int id)
        {
            ViewBag.ViewModel = this.GetFormJson(id);
            return View();
        }

        public ActionResult Md(int id)
        {
            //取出文章对应的多个标签Id
            var tagActive = _articleRepository.Queryable(u => u.DeleteMark == false && u.Id == id)
                .Select(u => u.TagArts.Select(r => r.TagId)).AsEnumerable().FirstOrDefault();

            ViewBag.TagActive = JsonHelper.SerializeObject(tagActive);
            ViewBag.ViewModel = this.GetFormJson(id);
            return View();
        }

        public ActionResult GetDataGrid(PageDto pag, string search = "")
        {
            return Content(_articleRepository.GetDataGrid(pag, search).ToJson());
        }

        /// <summary>
        /// 最新文章
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRecentArticle()
        {
            return Result(_articleRepository.GetRecentArticle(5));
        }

        /// <summary>
        /// 得到博客列表页面
        /// </summary>
        /// <param name="pag"></param>
        /// <returns></returns>
        [Skip]
        public ActionResult GetIndexGird(PageDto pag, string category = "", string tag = "", string archive = "")
        {
            if (category.IsNullOrEmpty() && tag.IsNullOrEmpty() && archive.IsNullOrEmpty())
            {
                throw new ArgumentNullException("参数异常!!tag" + tag);
            }
            if (pag.Sort.IsNullOrEmpty())
            {
                pag.Sort = "Id";
            }
            if (pag.Order.IsNullOrEmpty())
            {
                pag.Order = "desc";
            }
            DataGrid dg = _articleRepository.GetCategoryTagGrid(pag, category, tag, archive);
            return Content(dg.ToJson());
        }

        /// <summary>
        /// 文章新增，编辑，由于后期加了一个文章可以有多个标签，一个标签可对应多个文章，就是多对多的关系，
        /// 所以，这里就需要重新对数据进行处理后，提交数据库
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public override ActionResult Edit(ArticleViewModel viewModel, int id = 0)
        {
            if (!ModelState.IsValid)
            {
                return Error(ModelState.Values.FirstOrDefault(u => u.Errors.Count > 0)?.Errors[0].ErrorMessage);
            }

            string tagIds = Request["Tag"].ToString();
            List<TagArt> tagArtList = new List<TagArt> { };
            if (tagIds.IsNotNullOrEmpty())
            {
                foreach (var i in tagIds.Split(',').Select(u => Convert.ToInt32(u)).ToList())
                {
                    tagArtList.Add(new TagArt
                    {
                        TagId = i,
                        AId = id
                    });
                }
            }
            Article entity;
            if (id == 0)
            {
                entity = Mapper.Map<ArticleViewModel, Article>(viewModel);
                entity.Create();
                entity.TagArts = tagArtList;
                entity.Archive = DateTime.Now.ToString("yyy年MM月");
                _articleRepository.Insert(entity);
            }
            else
            {
                var tEntityModel = _articleRepository.FindEntity(id);

                if (tEntityModel == null)
                    return Error($"Id为{id}未找到任何类型为{viewModel.GetType().Name}的实体对象");

                entity = Mapper.Map(viewModel, tEntityModel);

                entity.Modify(id);
                using (var db = new RepositoryBase().BeginTrans())
                {
                    db.Update(entity);
                    db.Delete<TagArt>(u => u.AId == id);
                    db.Insert(tagArtList);
                    db.Commit();
                }
            }
            //要根据新增或修改的文章类型，来判断是否删除缓存中的数据

            if (entity.FId != null)
            {
                string itemCode = _itemsDetailRepository.Queryable(u => u.Id == entity.FId).Select(r => r.ItemCode).FirstOrDefault();
                if (itemCode != null)
                {
                    if (itemCode.Equals(CategoryCode.Justfun.ToString()))
                    {
                        RedisHelper.KeyDeleteAsync(string.Format(ConstHelper.App, "JustFun"));
                    }
                    if (itemCode.Equals(CategoryCode.Feelinglife.ToString()))
                    {
                        RedisHelper.KeyDeleteAsync(string.Format(ConstHelper.App, "FellLife"));
                    }
                }
            }
            RedisHelper.KeyDeleteAsync(string.Format(ConstHelper.Article, "GetRecentArticle"));
            RedisHelper.KeyDelete(string.Format(ConstHelper.Article, "archiveFile"));

            return Success();
        }
    }
}