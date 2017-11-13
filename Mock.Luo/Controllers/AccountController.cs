using EntityFramework.Extensions;
using Mock.Code;
using Mock.Data;
using Mock.Data.Models;
using Mock.Domain;
using Mock.Luo.Generic.Helper;
using Mock.Luo.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        private readonly IRedisHelper _redisHelper;
        private readonly IMailHelper _imailHelper;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IGuestBookRepository _guestBookRepository;
        public AccountController(IRedisHelper _redisHelper, IMailHelper _imailHelper, IAppUserRepository _appUserRepository, IReviewRepository _reviewRepository, IGuestBookRepository _guestBookRepository)
        {
            this._redisHelper = _redisHelper;
            this._imailHelper = _imailHelper;
            this._appUserRepository = _appUserRepository;
            this._reviewRepository = _reviewRepository;
            this._guestBookRepository = _guestBookRepository;
        }



        /// <summary>
        /// 帐号信息设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Set()
        {
            int? userid = op.CurrentUser.UserId;
            var viewModel = _appUserRepository.IQueryable(r => r.Id == userid).Select(r => new
            {
                r.LoginName,
                r.Email,
                r.QQ,
                r.EmailIsValid,
                r.Phone,
                r.NickName,
                r.Avatar,
                r.Gender,
                r.PersonSignature,
                r.PersonalWebsite,
                PwdIsSet = r.LoginPassword == null ? false : true
            }).FirstOrDefault();

            ViewBag.viewModel = JsonHelper.SerializeObject(viewModel);

            return View();
        }

        /// <summary>
        /// 我的评论
        /// </summary>
        /// <returns></returns>
        public ActionResult Comment()
        {
            return View();
        }

        /// <summary>
        /// 我的留言
        /// </summary>
        /// <returns></returns>
        public ActionResult GuestBook()
        {
            return View();
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <returns></returns>
        public ActionResult Collect()
        {
            return View();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            OperatorProvider op = OperatorProvider.Provider;
            op.RemoveCurrent();

            return RedirectToAction("Index", "App");
        }

        /// <summary>
        /// 根据邮箱发送四位验证码，并将token返回给前端
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>

        public ActionResult GetActiveCode(string Email)
        {
            if (Email.IsNullOrEmpty())
            {
                return Error("Email不能为空!!!");
            }
            if (!Validate.IsEmail(Email))
            {
                return Error("Email格式不正确!!!");
            }
            int? Id = op.CurrentUser.UserId;

            //bool EmailIsValid = _appUserRepository.IQueryable(u => u.Id == Id).Select(r => r.EmailIsValid).FirstOrDefault();

            //if (EmailIsValid)
            //{
            //    return Error("您已经绑定成功了，请不要重复绑定！！！");
            //}

            //为了安全，一个用户IP10分钟内只能请求此接口3次

            string IP = Net.Ip;
            int count = _redisHelper.StringGet<int>(IP);

            if (count >= 3)
            {
                return Error("请求过于频繁，请稍后再试！");
            }

            count += 1;

            _redisHelper.StringSet(IP, count, new TimeSpan(0, 10, 0));

            var amm = _appUserRepository.IsRepeat(new AppUser
            {
                Id = Id,
                Email = Email
            });
            if (amm.state.Equals(ResultType.error.ToString()))
            {
                return Error("该邮箱已被绑定其他用户绑定!");
            }

            //缓存1小时
            TimeSpan saveTime = new TimeSpan(1, 0, 0);

            //生成token
            string token = Utils.GuId();
            string rand4Num = Utils.RndNum(4);

            //redis缓存绑定邮箱随机token作为键，email作为值，随机u为键，当前登录id为值
            _redisHelper.StringSet(token, Email, saveTime);
            _redisHelper.StringSet(token + Email, rand4Num, saveTime);

            EmailViewModel emailViewModel = new EmailViewModel
            {
                ToUserName = Email,
                Code = rand4Num
            };

            string body = UiHelper.FormatEmail(emailViewModel, "SendCodeTemplate");
            _imailHelper.SendByThread(Email, "[、天上有木月博客] 邮箱激活通知", body);

            return Success("邮箱发送成功，请查收", token);
        }

        /// <summary>
        /// 生成一个token,发送邮箱给用户邮箱  
        /// /// </summary>
        /// <returns></returns>
        public ActionResult ActiveSendEmail(string Email)
        {
            if (Email.IsNullOrEmpty())
            {
                return Error("Email不能为空!!!");
            }
            if (!Validate.IsEmail(Email))
            {
                return Error("Email格式不正确!!!");
            }
            //缓存1小时
            TimeSpan saveTime = new TimeSpan(1, 0, 0);

            //生成token
            string token = Utils.GuId();
            string u = Utils.GuId();

            //redis缓存绑定邮箱随机token作为键，email作为值，随机u为键，当前登录id为值
            _redisHelper.StringSet(token, Email, saveTime);
            _redisHelper.StringSet(u, op.CurrentUser.UserId, saveTime);

            string rootUrl = Request.Url.Authority;

            EmailViewModel emailViewModel = new EmailViewModel
            {
                ToUserName = Email,
                Link = "http://" + rootUrl + "/App/ActiveEmail?t=" + token + "&u=" + u
            };
            string body = UiHelper.FormatEmail(emailViewModel, "ActiveEmailTemplate");

            _imailHelper.SendByThread(Email, "[、天上有木月博客] 邮箱激活通知", body);

            return Success("邮箱发送成功，请查收");
        }

        /// <summary>
        /// 修改email，需要上次缓存的验证码和token
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="EmailToken"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public ActionResult SetEmail(string Email, string EmailToken, string Code)
        {

            if (Code.IsNullOrEmpty())
            {
                return Error("验证码不能为空");
            }
            if (EmailToken.IsNullOrEmpty())
            {
                return Error("邮件标识符异常，请重新获取验证码！");
            }

            string email = _redisHelper.StringGet(EmailToken);

            if (email.IsNullOrEmpty())
            {
                return Error("您的操作已过期，请重试！");
            }

            if (Email.IsNullOrEmpty() || !email.Equals(Email))
            {
                return Error("邮箱参数异常");
            }
            string code = _redisHelper.StringGet(EmailToken + email);
            if (code.IsNullOrEmpty())
            {
                return Error("您的操作已过期，请重试！");
            }

            if (!Code.Equals(code))
            {
                return Error("您输入的验证码不正确，请重试!");
            }

            _appUserRepository.Update(_appUserRepository.IQueryable(u => u.Id == op.CurrentUser.UserId), u => new AppUser
            {
                Email = Email,
                EmailIsValid = true
            });
            _redisHelper.KeyDeleteAsync(EmailToken);
            _redisHelper.KeyDeleteAsync(EmailToken + Email);

            return Success("绑定成功！");
        }


        /// <summary>
        /// 设置用户名|仅一次，且不重复
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public ActionResult SetLoginName(string LoginName)
        {

            bool nameValidate = Validate.IsValidUserName(LoginName);
            if (!nameValidate)
            {
                return Error("您填写的用户名称不合法！");
            }
            int? UserId = op.CurrentUser.UserId;

            //判断当前用户名是否存在重复
            var amm = _appUserRepository.IsRepeat(new AppUser
            {
                Id = UserId,
                LoginName = LoginName
            });
            //重复直接返回
            if (amm.state.Equals(ResultType.error.ToString()))
            {
                return Result(amm);
            }
            var tempIQueryable = _appUserRepository.IQueryable(u => u.Id == UserId && u.DeleteMark == false);
            string loginNameMs = tempIQueryable.Select(r => r.LoginName).FirstOrDefault();
            //只有当前id查询出的用户名为空，才表明未设置过用户名
            if (loginNameMs.IsNullOrEmpty())
            {
                _appUserRepository.Update(tempIQueryable, u => new AppUser
                {
                    LoginName = LoginName
                });
            }
            else
            {
                return Error("您已经设置过用户名了，这个不能设置第二次");
            }

            return Success();
        }



        public ActionResult SaveUserInfo(AppUser userEntity)
        {
            if (!ModelState.IsValid)
            {
                return Error(ModelState.Values.Where(u => u.Errors.Count > 0).FirstOrDefault().Errors[0].ErrorMessage);
            }
            if (userEntity.NickName.IsNullOrEmpty())
            {
                return Error("昵称为必填项");
            }

            if (!Validate.IsValidUserName(userEntity.NickName))
            {
                return Error("昵称不合法，给我换、");
            }
            OperatorModel oUserModel = op.CurrentUser;

            userEntity.Modify(oUserModel.UserId);
            //_appUserRepository.IQueryable(u => u.Id == oUserModel.UserId).Update(u => userEntity);

            _appUserRepository.Update(userEntity, "QQ", "Phone", "NickName", "Gender", "PersonSignature", "PersonalWebsite");

            oUserModel.NickName = userEntity.NickName;
            op.CurrentUser = oUserModel;

            return Success();
        }

        /// <summary>
        /// 保存用户头像| 用于上传头像的回调
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveAvatar(string Avatar)
        {
            int? id = op.CurrentUser.UserId;
            _appUserRepository.Update(_appUserRepository.IQueryable(u => u.Id == id), r => new AppUser
            {
                Avatar = Avatar
            });
            OperatorModel oUserModel = op.CurrentUser;
            oUserModel.Avatar = Avatar;
            op.CurrentUser = oUserModel;

            return Success();
        }


        public ActionResult ResetPwd(string LoginPassword, string newPwd)
        {

            if (LoginPassword.IsNullOrEmpty())
            {
                return Error("旧密码不能为空");
            }
            if (newPwd.IsNullOrEmpty())
            {
                return Error("新密码不能为空");
            }

            int userId = (int)op.CurrentUser.UserId;

            AppUser userEntity = _appUserRepository.FindEntity(userId);
            if (userEntity != null)
            {
                string dbPwd = Md5.md5(DESEncrypt.Encrypt(LoginPassword.ToLower(), userEntity.UserSecretkey).ToLower(), 32).ToLower();
                //当后台密码为空时，说明未设置密码，将newPwd这个字段更新到密码字段。当后台密码，与旧密码一致，重置密码
                if (userEntity.LoginPassword.IsNullOrEmpty() || userEntity.LoginPassword.Equals(dbPwd))
                {
                    _appUserRepository.ResetPassword(userEntity, newPwd);
                    return Success("您的密码已经设置成功，请牢记你的密码噢！");
                }
                else
                {
                    return Error("你的旧密码填写不对，无法重置密码！");
                }

            }
            else
            {
                return Error("当前用户不存在、、");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCommentGrid(Pagination pag)
        {
            int? userId = op.CurrentUser.UserId;
           DataGrid dg= _reviewRepository.GetDataGrid(u => u.CreatorUserId == userId, pag, "", 0);

            return Result(dg);
        }

        public ActionResult GetGuestGrid(Pagination pag)
        {
            int? userId = op.CurrentUser.UserId;
            DataGrid dg = _guestBookRepository.GetDataGrid(u => u.CreatorUserId == userId, pag,"");
            return Result(dg);
        }

    }
}