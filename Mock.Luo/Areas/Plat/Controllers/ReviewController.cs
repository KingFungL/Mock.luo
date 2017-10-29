using Autofac;
using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using Mock.Domain;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class ReviewController : BaseController
    {
        // GET: Plat/Review

        private readonly IReviewRepository _reviewRepositroy;
        private readonly IArticleRepository _articleRepository;
        public ReviewController(IReviewRepository reviewRepositroy, IArticleRepository articleRepository)
        {
            this._reviewRepositroy =reviewRepositroy;
            this._articleRepository = articleRepository;
        }
        /// <summary>
        /// 得到最新的10条评论的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRecentReView()
        {
            var rows = _reviewRepositroy.GetRecentReview(10);
            return Result(rows);
        }
        [Skip]
        public ActionResult GetDataGrid(Pagination pag, string Email = "", int AId = 0)
        {
            if (pag.sort.IsNullOrEmpty())
            {
                pag.sort = "Id";
            }
            if (pag.order.IsNullOrEmpty())
            {
                pag.order = "desc";
            }
            if (pag.limit > 10)
            {
                pag.limit = 10;
            }
            return Result(_reviewRepositroy.GetDataGrid(pag, Email, AId));
        }

        /// <summary>
        /// 审核、拉黑评论
        /// </summary>
        /// <param name="IsAduit"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Aduit(bool IsAduit, int Id)
        {
            Review entity = new Review { Id = Id, IsAduit = IsAduit };
            entity.Modify(Id);
            _reviewRepositroy.Update(entity, "IsAduit", "LastModifyUserId", "LastModifyTime");
            return Success(IsAduit ? "审核成功！" : "拉黑成功！");
        }

        [Skip]
        public ActionResult Add(Review reViewModel)
        {
            if (reViewModel.AuEmail.IsNullOrEmpty())
            {
                return Error("Email不能为空！");
            }
            else if(!Validate.IsEmail(reViewModel.AuEmail))
            {
                return Error("邮箱格式不正确！");
            }
            if (reViewModel.AuName.IsNullOrEmpty())
            {
                return Error("用户昵称不能为空！");
            }
            if (!ModelState.IsValid)
            {
                return Error(ModelState.Values.Where(u=>u.Errors.Count>0).FirstOrDefault().Errors[0].ErrorMessage);
            }
            OperatorProvider op = OperatorProvider.Provider;

            //未登录状态下，将生成一个随机头像
            if (op.CurrentUser==null)
            {
                reViewModel.HeadHref ="/Content/user/"+ new Random(DateTime.Now.Second).Next(1,361)+".png";
            }
            else
            {
            }

            reViewModel.Create();

            reViewModel.System =Net.GetOSNameByUserAgent(Request.UserAgent);
            reViewModel.UserHost = Net.Host;
            reViewModel.Ip = Net.Ip;
            reViewModel.GeoPosition = Net.GetLocation(reViewModel.Ip);
            reViewModel.Agent = Net.Browser;
            reViewModel.IsAduit = true;

            _reviewRepositroy.Insert(reViewModel);

            var artEntity=_articleRepository.IQueryable(u => u.Id == reViewModel.AId).FirstOrDefault();
            artEntity.CommentQuantity += 1;

            _articleRepository.Update(artEntity, "CommentQuantity");

            return Success("留言成功");
        }

    }

}