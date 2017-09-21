using Autofac;
using Mock.Code;
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
    public class ReviewController : CrudController<Review, ReViewModel>
    {
        // GET: Plat/Review

        private readonly IReviewRepository _service;
        public ReviewController(IReviewRepository service, IComponentContext container) : base(container)
        {
            this._service = service;
        }
        /// <summary>
        /// 得到最新的10条评论的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRecentReView()
        {
            var rows = _service.GetRecentReview(10);
            return Result(rows);
        }

        public ActionResult GetDataGrid(Pagination pag, string Email = "", int AId = 0)
        {
            return Result(_service.GetDataGrid(pag, Email, AId));
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
            _service.Update(entity, "IsAduit", "LastModifyUserId", "LastModifyTime");
            return Success(IsAduit ? "审核成功！" : "拉黑成功！");
        }
    }
}