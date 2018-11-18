using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Mock.Code;
using Mock.Code.Helper;
using Mock.Code.Mail;
using Mock.Code.Net;
using Mock.Code.Security;
using Mock.Code.Validate;
using Mock.Code.Web;
using Mock.Data.AppModel;
using Mock.Data.Models;
using Mock.Domain.Interface;
using Mock.Luo.Models;

namespace Mock.Luo.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account

        #region Construct
        private readonly IRedisHelper _redisHelper;
        private readonly IMailHelper _imailHelper;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IGuestBookRepository _guestBookRepository;
        public AccountController(IRedisHelper redisHelper, IMailHelper imailHelper, IAppUserRepository appUserRepository, IReviewRepository reviewRepository, IGuestBookRepository guestBookRepository)
        {
            this._redisHelper = redisHelper;
            this._imailHelper = imailHelper;
            this._appUserRepository = appUserRepository;
            this._reviewRepository = reviewRepository;
            this._guestBookRepository = guestBookRepository;
        }

        #endregion

        /// <summary>
        /// 帐号信息设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Set()
        {
            int? userid = Op.CurrentUser.UserId;
            var viewModel = _appUserRepository.Queryable(r => r.Id == userid).Select(r => new
            {
                r.LoginName,
                r.Email,
                QQ = r.Qq,
                r.EmailIsValid,
                r.Phone,
                r.NickName,
                r.Avatar,
                r.Gender,
                r.PersonSignature,
                r.PersonalWebsite,
                PwdIsSet = r.LoginPassword != null,
                isBindQQ = r.AppUserAuths.Select(u => u.IdentityType == "QQ").Any()
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
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "App");
        }

        /// <summary>
        /// 根据邮箱发送四位验证码，并将token返回给前端
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        public ActionResult GetActiveCode(string email)
        {
            if (email.IsNullOrEmpty())
            {
                return Error("Email不能为空!!!");
            }
            if (!Validate.IsEmail(email))
            {
                return Error("Email格式不正确!!!");
            }
            int id = Op.CurrentUser.UserId;

            //bool EmailIsValid = _appUserRepository.IQueryable(u => u.Id == Id).Select(r => r.EmailIsValid).FirstOrDefault();

            //if (EmailIsValid)
            //{
            //    return Error("您已经绑定成功了，请不要重复绑定！！！");
            //}

            //为了安全，一个用户IP10分钟内只能请求此接口3次

            string ip = Net.Ip;
            int count = _redisHelper.StringGet<int>(ip);

            if (count >= 3)
            {
                return Error("请求过于频繁，请稍后再试！");
            }

            count += 1;

            _redisHelper.StringSet(ip, count, new TimeSpan(0, 10, 0));

            var amm = _appUserRepository.IsRepeat(new AppUser
            {
                Id = id,
                LoginName=Utils.GuId(),
                Email = email
            });
            if (amm.State.Equals(ResultType.Error.ToString()))
            {
                return Error("该邮箱已被绑定其他用户绑定!");
            }

            //缓存1小时
            TimeSpan saveTime = new TimeSpan(1, 0, 0);

            //生成token
            string token = Utils.GuId();
            string rand4Num = Utils.RndNum(4);

            //redis缓存绑定邮箱随机token作为键，email作为值，随机u为键，当前登录id为值
            _redisHelper.StringSet(token, email, saveTime);
            _redisHelper.StringSet(token + email, rand4Num, saveTime);

            EmailViewModel emailViewModel = new EmailViewModel
            {
                ToUserName = email,
                Code = rand4Num
            };

            string body = UiHelper.FormatEmail(emailViewModel, "SendCodeTemplate");
            _imailHelper.SendByThread(email, "[、天上有木月博客] 邮箱激活通知", body);

            return Success("邮箱发送成功，请查收", token);
        }

        /// <summary>
        /// 生成一个token,发送邮箱给用户邮箱  
        /// /// </summary>
        /// <returns></returns>
        public ActionResult ActiveSendEmail(string email)
        {
            if (email.IsNullOrEmpty())
            {
                return Error("Email不能为空!!!");
            }
            if (!Validate.IsEmail(email))
            {
                return Error("Email格式不正确!!!");
            }
            //缓存1小时
            TimeSpan saveTime = new TimeSpan(1, 0, 0);

            //生成token
            string token = Utils.GuId();
            string u = Utils.GuId();

            //redis缓存绑定邮箱随机token作为键，email作为值，随机u为键，当前登录id为值
            _redisHelper.StringSet(token, email, saveTime);
            _redisHelper.StringSet(u, Op.CurrentUser.UserId, saveTime);

            string rootUrl = Request.Url.Authority;

            EmailViewModel emailViewModel = new EmailViewModel
            {
                ToUserName = email,
                Link = "http://" + rootUrl + "/App/ActiveEmail?t=" + token + "&u=" + u
            };
            string body = UiHelper.FormatEmail(emailViewModel, "ActiveEmailTemplate");

            _imailHelper.SendByThread(email, "[、天上有木月博客] 邮箱激活通知", body);

            return Success("邮箱发送成功，请查收");
        }

        /// <summary>
        /// 修改email，需要上次缓存的验证码和token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="emailToken"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult SetEmail(string email, string emailToken, string code)
        {

            if (code.IsNullOrEmpty())
            {
                return Error("验证码不能为空");
            }
            if (emailToken.IsNullOrEmpty())
            {
                return Error("邮件标识符异常，请重新获取验证码！");
            }

            string emailByToken = _redisHelper.StringGet(emailToken);

            if (emailByToken.IsNullOrEmpty())
            {
                return Error("您的操作已过期，请重试！");
            }

            if (emailByToken.IsNullOrEmpty() || !email.Equals(emailByToken))
            {
                return Error("邮箱参数异常");
            }
            string codeNew = _redisHelper.StringGet(emailToken + email);
            if (codeNew.IsNullOrEmpty())
            {
                return Error("您的操作已过期，请重试！");
            }

            if (!codeNew.Equals(code))
            {
                return Error("您输入的验证码不正确，请重试!");
            }

            _appUserRepository.Update(_appUserRepository.Queryable(u => u.Id == Op.CurrentUser.UserId), u => new AppUser
            {
                Email = email,
                EmailIsValid = true
            });
            _redisHelper.KeyDeleteAsync(emailToken);
            _redisHelper.KeyDeleteAsync(emailToken + email);

            return Success("绑定成功！");
        }


        /// <summary>
        /// 设置用户名|仅一次，且不重复
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public ActionResult SetLoginName(string loginName)
        {

            bool nameValidate = Validate.IsValidUserName(loginName);
            if (!nameValidate)
            {
                return Error("您填写的用户名称不合法！");
            }
            int userId = Op.CurrentUser.UserId;

            //判断当前用户名是否存在重复
            var amm = _appUserRepository.IsRepeat(new AppUser
            {
                Id = userId,
                LoginName = loginName
            });
            //重复直接返回
            if (amm.State.Equals(ResultType.Error.ToString()))
            {
                return Result(amm);
            }
            var tempIQueryable = _appUserRepository.Queryable(u => u.Id == userId && u.DeleteMark == false);
            string loginNameMs = tempIQueryable.Select(r => r.LoginName).FirstOrDefault();
            //只有当前id查询出的用户名为空，才表明未设置过用户名
            if (loginNameMs.IsNullOrEmpty())
            {
                _appUserRepository.Update(tempIQueryable, u => new AppUser
                {
                    LoginName = loginName
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
                return Error(ModelState.Values.FirstOrDefault(u => u.Errors.Count > 0)?.Errors[0].ErrorMessage);
            }
            if (userEntity.NickName.IsNullOrEmpty())
            {
                return Error("昵称为必填项");
            }

            if (!Validate.IsValidUserName(userEntity.NickName))
            {
                return Error("昵称不合法，给我换、");
            }
            OperatorModel oUserModel = Op.CurrentUser;

            userEntity.Modify(oUserModel.UserId);
            //_appUserRepository.IQueryable(u => u.Id == oUserModel.UserId).Update(u => userEntity);

            _appUserRepository.Update(userEntity, "Qq", "Phone", "NickName", "Gender", "PersonSignature", "PersonalWebsite");

            oUserModel.NickName = userEntity.NickName;
            Op.CurrentUser = oUserModel;

            return Success();
        }

        /// <summary>
        /// 保存用户头像| 用于上传头像的回调
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveAvatar(string avatar)
        {
            int? id = Op.CurrentUser.UserId;
            _appUserRepository.Update(_appUserRepository.Queryable(u => u.Id == id), r => new AppUser
            {
                Avatar = avatar
            });
            OperatorModel oUserModel = Op.CurrentUser;
            oUserModel.Avatar = avatar;
            Op.CurrentUser = oUserModel;

            return Success();
        }

        [HttpPost]
        public ActionResult ResetPwd(string newPwd, string loginPassword = "")
        {

            int userId = (int)Op.CurrentUser.UserId;

            if (newPwd.IsNullOrEmpty())
            {
                return Error("新密码不能为空");
            }

            AppUser userEntity = _appUserRepository.FindEntity(userId);
            if (userEntity != null)
            {
                //当旧密码为空时，直接重置
                if (userEntity.LoginPassword.IsNullOrEmpty())
                {
                    _appUserRepository.ResetPassword(userEntity, newPwd);
                    return Success("您的密码已经设置成功，请牢记你的密码噢！");
                }
                else
                {

                    if (loginPassword.IsNullOrEmpty())
                    {
                        return Error("旧密码不能为空");
                    }
                    if (userEntity.UserSecretkey.IsNullOrEmpty())
                    {
                        return Error("用户密钥丢失，请联系管理员重置密码");
                    }
                    string dbPwd = Md5Helper.Md5(DesEncrypt.Encrypt(loginPassword.ToLower(), userEntity.UserSecretkey).ToLower(), 32).ToLower();

                    if (userEntity.LoginPassword.Equals(dbPwd))
                    {
                        _appUserRepository.ResetPassword(userEntity, newPwd);
                        return Success("您的密码已经设置成功，请牢记你的密码噢！");
                    }
                    else
                    {
                        return Error("你的旧密码填写不对，无法重置密码！");
                    }
                }
            }
            else
            {
                return Error("当前用户不存在");
            }
        }

        /// <summary>
        /// 得到自己的评论列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCommentGrid(PageDto pag)
        {
            int? userId = Op.CurrentUser.UserId;
            DataGrid dg = _reviewRepository.GetDataGrid(u => u.CreatorUserId == userId, pag, "", 0);

            return Result(dg);
        }

        /// <summary>
        /// 得到自己的留言列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGuestGrid(PageDto pag)
        {
            int? userId = Op.CurrentUser.UserId;
            DataGrid dg = _guestBookRepository.GetDataGrid(u => u.CreatorUserId == userId, pag, "");
            return Result(dg);
        }

    }
}