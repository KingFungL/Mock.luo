using Autofac;
using Mock.Code;
using Mock.Code.Helper;
using Mock.Data;
using Mock.Data.Models;
using Mock.Domain;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using Mock.Luo.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class GuestBookController : CrudController<GuestBook, GuestBookViewModel>
    {
        // GET: Plat/GuestBook
        private readonly IGuestBookRepository _guestBookRepository;
        private readonly IMailHelper _imailHelper;
        public GuestBookController(IGuestBookRepository _guestBookRepository, IMailHelper _imailHelper, IComponentContext container) : base(container)
        {
            this._guestBookRepository = _guestBookRepository;
            this._imailHelper = _imailHelper;
        }
        [Skip]
        public ActionResult GetDataGrid(Pagination pag, string search = "")
        {
            if (pag.sort.IsNullOrEmpty())
            {
                pag.sort = "Id";
            }
            if (pag.order.IsNullOrEmpty())
            {
                pag.order = "desc";
            }
            if (pag.limit > 20)
            {
                pag.limit = 10;
            }
            return Result(_guestBookRepository.GetDataGrid(pag, search));
        }

        /// <summary>
        /// 审核、拉黑留言
        /// </summary>
        /// <param name="IsAduit"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Aduit(bool IsAduit, int Id)
        {
            GuestBook entity = new GuestBook { Id = Id, IsAduit = IsAduit };
            entity.Modify(Id);
            _guestBookRepository.Update(entity, "IsAduit", "LastModifyUserId", "LastModifyTime");
            return Success(IsAduit ? "审核成功！" : "拉黑成功！");
        }

        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Skip]
        public ActionResult Add(GuestBook viewModel)
        {
            if (viewModel.AuEmail.IsNullOrEmpty())
            {
                return Error("Email不能为空！");
            }
            else if (!Validate.IsEmail(viewModel.AuEmail))
            {
                return Error("邮箱格式不正确！");
            }
            if (viewModel.AuName.IsNullOrEmpty())
            {
                return Error("用户昵称不能为空！");
            }
            viewModel.Ip = Net.Ip;

            string userIp = _redisHelper.StringGet(string.Format(ConstHelper.GuestBook, "IP-" + viewModel.Ip));

            if (userIp.IsNotNull())
            {
                return Error("您的留言太频繁了，请稍后再试!!!");
            }

            if (!ModelState.IsValid)
            {
                return Error(ModelState.Values.Where(u => u.Errors.Count > 0).FirstOrDefault().Errors[0].ErrorMessage);
            }
            OperatorProvider op = OperatorProvider.Provider;

            //未登录状态下，将生成一个随机头像
            if (op.CurrentUser == null)
            {
                viewModel.Avatar = "/Content/user/" + new Random(DateTime.Now.Second).Next(1, 361) + ".png";
            }
            else
            {
            }

            viewModel.Create();

            viewModel.System = Net.GetOSNameByUserAgent(Request.UserAgent);
            viewModel.UserHost = Net.Host;
            viewModel.GeoPosition = Net.GetLocation(viewModel.Ip);
            viewModel.Agent = Net.Browser;
            viewModel.IsAduit = true;

            _guestBookRepository.Insert(viewModel);

            //缓存用户ip一分钟，用于频繁操作警告
            _redisHelper.StringSet(string.Format(ConstHelper.GuestBook, "IP-" + viewModel.Ip), 1, new TimeSpan(0, 1, 0));

            //留言成功后，给博主的email邮箱发送信息

            _imailHelper.SendByThread(Configs.GetValue("DeveloperEmail"), "有人在你的（、天上有木月）博客发表的留言", this.FormatSendToDeveloper(new EmailViewModel
            {
                Title = "有留言了！！！",
                ToUserName = "、天上有木月",
                FromUserName = viewModel.AuName,
                Date = viewModel.CreatorTime.ToDateTimeString(),
                Content = viewModel.Text
            }));

            return Success("留言成功");
        }


        /// <summary>
        /// 将留言实体格式化为邮箱body内容
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public string FormatSendToDeveloper(EmailViewModel viewModel)
        {

            string template = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Views/Generic/EmailTemplate.cshtml"));

            var body = Engine.Razor.RunCompile(template, "EmailTemplate", null, viewModel);

            return body;
        }
    }

}