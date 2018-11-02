using System;
using System.Linq;
using System.Web.Mvc;
using Mock.Data.Models;
using Mock.Domain.Interface;
using Mock.Luo.Controllers;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class PointArticleController : BaseController
    {
        // GET: Plat/PointArticle

        private readonly IPointArticleRepository _service;
        private readonly IArticleRepository _articleRepository;
        public PointArticleController(IPointArticleRepository service, IArticleRepository articleRepository)
        {
            this._service = service;
            this._articleRepository = articleRepository;
        }

        //根据文章id得到点赞人信息
        public ActionResult GetDataGrid(int aId)
        {
            return Result(_service.GetDataGrid(aId));
        }

        //用户点赞
        public ActionResult Edit(PointArticle entry)
        {
            entry.AddTime = DateTime.Now;
            entry.UserId = (int)Op.CurrentUser.UserId;

            var pointCount = _service.Queryable(u => u.UserId == entry.UserId && u.AId == entry.AId).Count();
            //点当前文章已经被点赞时，用户再次点赞，
            if (pointCount > 0)
            {
                return Error("你已经点过赞了！");

                //_service.Delete(u => u.UserId == entry.UserId && u.AId == entry.AId);
            }
            else
            {
                _service.Insert(entry);
                int? pointQuantity=_articleRepository.Queryable(r => r.Id == entry.AId).Select(r => r.PointQuantity).FirstOrDefault();

                _articleRepository.Update(new Article { Id = entry.AId, PointQuantity = pointQuantity + 1 }, "PointQuantity");
            }

            return Success("成功点赞");
        }

    }
}