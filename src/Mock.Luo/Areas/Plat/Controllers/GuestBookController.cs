using System;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Mock.Code.Attribute;
using Mock.Code.Configs;
using Mock.Code.Extend;
using Mock.Code.Helper;
using Mock.Code.Mail;
using Mock.Code.Net;
using Mock.Code.Validate;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Domain.Interface;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using Mock.Luo.Models;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class GuestBookController : CrudController<GuestBook, GuestBookViewModel>
    {
        // GET: Plat/GuestBook
        private readonly IGuestBookRepository _guestBookRepository;
        private readonly IMailHelper _imailHelper;
        public GuestBookController(IGuestBookRepository guestBookRepository, IMailHelper imailHelper, IComponentContext container) : base(container)
        {
            this._guestBookRepository = guestBookRepository;
            this._imailHelper = imailHelper;
        }
        [Skip]
        public ActionResult GetAduitDataGrid(PageDto pag, string search = "")
        {
            if (pag.Sort.IsNullOrEmpty())
            {
                pag.Sort = "Id";
            }
            if (pag.Order.IsNullOrEmpty())
            {
                pag.Order = "desc";
            }
            if (pag.Limit > 20)
            {
                pag.Limit = 10;
            }
            return Result(_guestBookRepository.GetDataGrid(u => u.IsAduit==true, pag, search));
        }

        public ActionResult GetDataGrid(PageDto pag, string search = "")
        {
            if (pag.Sort.IsNullOrEmpty())
            {
                pag.Sort = "Id";
            }
            if (pag.Order.IsNullOrEmpty())
            {
                pag.Order = "desc";
            }
            if (pag.Limit > 20)
            {
                pag.Limit = 10;
            }
            return Result(_guestBookRepository.GetDataGrid(u => true, pag, search));
        }

        /// <summary>
        /// 审核、拉黑留言
        /// </summary>
        /// <param name="isAduit"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Aduit(bool isAduit, int id)
        {
            GuestBook entity = new GuestBook { Id = id, IsAduit = isAduit };
            entity.Modify(id);
            _guestBookRepository.Update(entity, "IsAduit", "LastModifyUserId", "LastModifyTime");
            return Success(isAduit ? "审核成功！" : "拉黑成功！");
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

            string userIp = RedisHelper.StringGet(string.Format(ConstHelper.GuestBook, "IP-" + viewModel.Ip));

            if (userIp.IsNotNull())
            {
                return Error("您的留言太频繁了，请稍后再试!!!");
            }

            if (!ModelState.IsValid)
            {
                return Error(ModelState.Values.FirstOrDefault(u => u.Errors.Count > 0)?.Errors[0].ErrorMessage);
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

            viewModel.System = Net.GetOsNameByUserAgent(Request.UserAgent);
            viewModel.UserHost = Net.Host;
            viewModel.GeoPosition = Net.GetLocation(viewModel.Ip);
            viewModel.Agent = Net.Browser;
            viewModel.IsAduit = true;

            _guestBookRepository.Insert(viewModel);

            //缓存用户ip一分钟，用于频繁操作警告
            RedisHelper.StringSet(string.Format(ConstHelper.GuestBook, "IP-" + viewModel.Ip), 1, new TimeSpan(0, 1, 0));

            //留言成功后，给博主的email邮箱发送信息

            _imailHelper.SendByThread(Configs.GetValue("DeveloperEmail"), "有人在你的（、天上有木月）博客发表的留言", UiHelper.FormatEmail(new EmailViewModel
            {
                Title = "有留言了！！！",
                ToUserName = "、天上有木月",
                FromUserName = viewModel.AuName,
                Date = viewModel.CreatorTime.ToDateTimeString(),
                Content = viewModel.Text
            }, "EmailTemplate"));

            return Success("留言成功");
        }
    }

}