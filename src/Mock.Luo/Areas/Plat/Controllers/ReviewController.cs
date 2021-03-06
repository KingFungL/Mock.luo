﻿using Mock.Code.Attribute;
using Mock.Code.Extend;
using Mock.Code.Helper;
using Mock.Code.Mail;
using Mock.Code.Net;
using Mock.Code.Validate;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Domain.Interface;
using Mock.Luo.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class ReviewController : BaseController
    {
        // GET: Plat/Review

        #region Constructor
        private readonly IReviewRepository _reviewRepositroy;
        private readonly IArticleRepository _articleRepository;
        private readonly IRedisHelper _redisHelper;
        private readonly IMailHelper _mailHelper;

        public ReviewController(IReviewRepository reviewRepositroy,
            IArticleRepository articleRepository,
            IMailHelper mailHelper,
            IRedisHelper redisHelper)
        {
            this._reviewRepositroy = reviewRepositroy;
            this._articleRepository = articleRepository;
            this._redisHelper = redisHelper;
            this._mailHelper = mailHelper;
        }
        #endregion

        /// <summary>
        /// 得到最新的10条评论的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRecentReView()
        {
            var rows = _reviewRepositroy.GetRecentReview(10);
            return Result(rows);
        }

        /// <summary>
        /// 后台：评论内容
        /// </summary>
        /// <param name="pag"></param>
        /// <param name="email"></param>
        /// <param name="aId"></param>
        /// <returns></returns>
        public ActionResult GetDataGrid(PageDto pag, string email = "", int aId = 0)
        {
            return Result(_reviewRepositroy.GetDataGrid(ExtLinq.True<Review>(), pag, email, aId, 1));//所有
        }

        /// <summary>
        /// 审核、拉黑评论
        /// </summary>
        /// <param name="isAduit"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Aduit(bool isAduit, int id)
        {
            Review entity = new Review { Id = id, IsAduit = isAduit };
            entity.Modify(id);
            _reviewRepositroy.Update(entity, "IsAduit", "LastModifyUserId", "LastModifyTime");
            return Success(isAduit ? "审核成功！" : "拉黑成功！");
        }


        /// <summary>
        /// 前端：文章对应的评论分页数据
        /// </summary>
        /// <param name="pag">分页</param>
        /// <param name="aId">文章ID</param>
        /// <returns></returns>
        [Skip]
        public ActionResult GetReviewGrid(PageDto pag, int aId)
        {
            if (pag.Sort.IsNullOrEmpty())
            {
                pag.Sort = "Id";
            }
            if (pag.Order.IsNullOrEmpty())
            {
                pag.Order = "desc";
            }
            if (pag.Limit > 10)
            {
                pag.Limit = 10;
            }
            //审核通过的
            return Result(_reviewRepositroy.GetDataGrid(u => u.IsAduit == true, pag, "", aId, 0));
        }

        # region 前台用户评论文章
        /// <summary>
        /// 前台：用户评论文章
        /// </summary>
        /// <param name="reViewModel"></param>
        /// <returns></returns>
        [Skip]
        public ActionResult Add(Review reViewModel)
        {
            if (reViewModel.AuEmail.IsNullOrEmpty())
            {
                return Error("Email不能为空！");
            }
            else if (!Validate.IsEmail(reViewModel.AuEmail))
            {
                return Error("邮箱格式不正确！");
            }
            if (reViewModel.AuName.IsNullOrEmpty())
            {
                return Error("用户昵称不能为空！");
            }
            if (!ModelState.IsValid)
            {
                return Error(ModelState.Values.FirstOrDefault(u => u.Errors.Count > 0)?.Errors[0].ErrorMessage);
            }
            OperatorProvider op = OperatorProvider.Provider;

            //未登录状态下，将生成一个随机头像
            if (op.CurrentUser == null)
            {
                reViewModel.Avatar = "/Content/user/" + new Random(DateTime.Now.Second).Next(1, 361) + ".png";
            }
            else
            {
            }

            reViewModel.Create();

            reViewModel.System = Net.GetOsNameByUserAgent(Request.UserAgent);
            reViewModel.UserHost = Net.Host;
            reViewModel.Ip = Net.Ip;
            reViewModel.GeoPosition = Net.GetLocation(reViewModel.Ip);
            reViewModel.Agent = Net.Browser;
            reViewModel.IsAduit = true;

            _reviewRepositroy.Insert(reViewModel);

            var artEntity = _articleRepository.Queryable(u => u.Id == reViewModel.AId).FirstOrDefault();
            if (artEntity != null)
            {
                artEntity.CommentQuantity += 1;

                _articleRepository.Update(artEntity, "CommentQuantity");
            }

            _redisHelper.KeyDeleteAsync(string.Format(ConstHelper.Review, "GetRecentReview"));

            return Success("吐槽成功");
        }
        #endregion

    }

}