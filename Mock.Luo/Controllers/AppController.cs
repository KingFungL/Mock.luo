using Mock.Code;
using Mock.Code.Helper;
using Mock.Data;
using Mock.Data.Dto;
using Mock.Data.Extensions;
using Mock.Data.Models;
using Mock.Domain;
using Mock.Domain.Interface;
using Mock.Luo.Models;
using QConnectSDK;
using QConnectSDK.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Mock.Luo.Controllers
{
    [Skip]
    public class AppController : BaseController
    {
        #region Constructor
        private readonly IItemsDetailRepository _itemsDetailRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IRedisHelper _iredisHelper;
        private readonly IGuestBookRepository _gusetbookRepository;
        private readonly IAppUserAuthRepository _appAuthRepository;
        private readonly IAppUserRepository _appUserRepository;
        // GET: App
        public AppController(IItemsDetailRepository itemsDetailRepository,
            IArticleRepository articleRepository,
            IReviewRepository reviewRepository,
            IGuestBookRepository gusetbookRepository,
            IAppUserAuthRepository appAuthRepository,
            IAppUserRepository appUserRepository,
            IRedisHelper iredisHelper
            )
        {
            this._itemsDetailRepository = itemsDetailRepository;
            this._articleRepository = articleRepository;
            this._reviewRepository = reviewRepository;
            this._gusetbookRepository = gusetbookRepository;
            this._iredisHelper = iredisHelper;
            this._appAuthRepository = appAuthRepository;
            this._appUserRepository = appUserRepository;

        }
        #endregion

        #region 博客首页
        public override ActionResult Index()
        {
            //标签
            List<TreeSelectModel> tagItemsList = _itemsDetailRepository.GetCombobox("Tag");
            tagItemsList.RemoveAt(0);
            ViewData["Tag"] = tagItemsList;

            //最新文章
            List<ArtDetailDto> LatestArticles = _articleRepository.GetRecentArticle(5);
            ViewData["LatestArticles"] = LatestArticles;

            //最热文章
            List<Article> HotArticles = _articleRepository.GetHotArticle(8);
            ViewData["HotArticles"] = HotArticles;

            //吐槽
            List<ReplyDto> SpitslotList = _reviewRepository.GetRecentReview(8);
            ViewData["SpitslotList"] = SpitslotList;

            //统计
            SiteStatistics site = _articleRepository.GetSiteData();

            ViewData["Site"] = site;

            //轻松时刻 | 缓存
            ViewData["JustFun"] = _iredisHelper.UnitOfWork(string.Format(ConstHelper.App, "JustFun"), () =>
              {
                  List<ArtDetailDto> justFunList = _articleRepository.GetArticleList(_articleRepository.IQueryable(u => u.DeleteMark == false && u.ItemsDetail.ItemCode == CategoryCode.justfun.ToString())).OrderByDescending(u => u.Id).Take(5).ToList();
                  if (justFunList.Count > 0)
                  {
                      justFunList[0].Content = Server.UrlDecode(justFunList[0].Content);
                  }
                  return justFunList;
              });

            //人生感悟 | 缓存
            ViewData["FellLife"] = _iredisHelper.UnitOfWork(string.Format(ConstHelper.App, "FellLife"), () =>
            {
                List<ArtDetailDto> feLifeList = _articleRepository.GetArticleList(_articleRepository.IQueryable(u => u.DeleteMark == false && u.ItemsDetail.ItemCode == CategoryCode.feelinglife.ToString())).OrderByDescending(u => u.Id).Take(5).ToList();
                if (feLifeList.Count > 0)
                {
                    feLifeList[0].Content = Server.UrlDecode(feLifeList[0].Content);
                }
                return feLifeList;
            });

            return base.Index();
        }
        #endregion

        #region 博客文章详情页
        public override ActionResult Detail(int Id)
        {
            ArtDetailDto entry = _articleRepository.GetOneArticle(Id);
            if (entry == null) throw new ArgumentNullException("根据Id,我去查了，但文章就是未找到！");
            //找到当前的上一个，下一个的文章

            Expression<Func<Article, bool>> nextlambda = u => u.Id > Id && u.DeleteMark == false;

            var next = _articleRepository.IQueryable(nextlambda).FirstOrDefault();
            if (next != null)
            {
                entry.NextPage = new BaseDto
                {
                    Id = next.Id,
                    text = next.Title
                };
            }
            Expression<Func<Article, bool>> previouslabmda = u => u.Id < Id && u.DeleteMark == false;
            var pre = _articleRepository.IQueryable(previouslabmda).OrderByDescending(u => u.Id).FirstOrDefault();
            if (pre != null)
            {
                entry.PreviousPage = new BaseDto
                {
                    Id = pre.Id,
                    text = pre.Title
                };
            }

            IQueryable<Article> queryable = _articleRepository.IQueryable(u => u.Id == Id);
            _articleRepository.Update(queryable, u => new Article
            {
                ViewHits = entry.ViewHits + 1
            });

            entry.Content = Server.UrlDecode(entry.Content);
            ViewData["ArticleDto"] = entry;

            ViewBag.AId = Id;

            return base.View();
        }
        #endregion

        #region 分类文章|标签视图
        /// <summary>
        /// 分类文章|标签
        /// </summary>
        /// <param name="category"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ActionResult Category(string category = "", string tag = "", string archive = "")
        {
            if (category.IsNullOrEmpty() && tag.IsNullOrEmpty() && archive.IsNullOrEmpty())
            {
                throw new ArgumentNullException("参数不正确！！!");
            }
            Pagination pag = new Pagination
            {
                sort = "Id",
                order = "desc",
                limit = 10,
                offset = 0
            };
            DataGrid dg = _articleRepository.GetCategoryTagGrid(pag, category, tag, archive);
            string TypeName = "";
            if (category.IsNotNullOrEmpty() || tag.IsNotNullOrEmpty())
            {

                TypeName = _itemsDetailRepository.IQueryable(r => r.ItemCode == category || r.ItemCode == tag).Select(r => r.ItemName).FirstOrDefault();
            }
            else
            {
                TypeName = archive;
            }
            ViewBag.TypeName = TypeName.IsNullOrEmpty() ? "亲，您迷路了啊！|、天上有木月" : TypeName;

            ViewBag.ViewModel = dg.ToJson();

            return View();
        }
        #endregion

        #region 留言互动页面
        /// <summary>
        /// 留言互动页面
        /// </summary>
        /// <returns></returns>

        public ActionResult GuestBook()
        {
            Pagination pag = new Pagination
            {
                sort = "Id",
                order = "desc",
                limit = 10,
                offset = 0
            };
            DataGrid dg = _gusetbookRepository.GetDataGrid(pag, "");

            ViewBag.ViewModel = dg.ToJson();

            return View();
        }
        #endregion

        #region 获取文章相关内容：文章归档，置顶文章，分类，标签，相关文章，随机文章
        public ActionResult GetRelateList(int Id)
        {
            var ardList = _articleRepository.GetRelateDtoByAId(Id);

            return Result(ardList);
        }
        #endregion

        public ActionResult NotFound()
        {
            return View();
        }

        #region qq登录重定向页面
        /// <summary> 
        /// QQ登陆页面 
        /// </summary>
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            this.Session["ReturnUrl"] = returnUrl;
            var context = new QzoneContext();
            string state = Guid.NewGuid().ToString().Replace("-", "");
            Session["requeststate"] = state;
            string scope = "get_user_info,add_share,list_album,upload_pic,check_page_fans,add_t,add_pic_t,del_t,get_repost_list,get_info,get_other_info,get_fanslist,get_idolist,add_idol,del_idol,add_one_blog,add_topic,get_tenpay_addr";
            var authenticationUrl = context.GetAuthorizationUrl(state, scope);
            return new RedirectResult(authenticationUrl);
        }
        #endregion

        #region qq互联回调页面
        /// <summary> 
        /// 回调页面 
        /// </summary>
        public ActionResult QQConnect()
        {
            if (Request.Params["code"] != null)
            {
                QOpenClient qzone = null;

                var verifier = Request.Params["code"];
                var state = Request.Params["state"];
                string requestState = Session["requeststate"].ToString();

                if (state == requestState)
                {
                    qzone = new QOpenClient(verifier, state);
                    var currentUser = qzone.GetCurrentUser();
                    if (this.Session["QzoneOauth"] == null)
                    {
                        this.Session["QzoneOauth"] = qzone;
                    }


                    var openId = qzone.OAuthToken.OpenId;
                    var accessToken = qzone.OAuthToken.AccessToken;
                    var expiresAt = qzone.OAuthToken.ExpiresAt;
                    DateTime now = DateTime.Now;
                    AppUserAuth userAuth = _appAuthRepository.IQueryable(r => r.OpenId == openId && r.DeleteMark == false).FirstOrDefault();
                    AppUser appUserEntity;
                    //如果未找到一个openid存在，说明当前用户未使用qq第三方登录
                    if (userAuth == null)
                    {
                        appUserEntity = new AppUser
                        {
                            NickName = currentUser.Nickname,
                            Avatar = currentUser.Figureurl,
                            Gender = currentUser.Gender,
                            CreatorTime = now,
                            DeleteMark = false,
                            StatusCode = StatusCode.Enable.ToString(),
                            AppUserAuths = new List<AppUserAuth>
                            {
                                new AppUserAuth{
                                    IdentityType=IdentityType.QQ.ToString(),
                                    OpenId=openId,
                                    AccessToken=accessToken,
                                    ExpiresAt=expiresAt,
                                    CreatorTime=now,
                                    DeleteMark=false
                                }
                            }
                        };

                        _appUserRepository.Insert(appUserEntity);
                    }
                    else
                    {
                        userAuth.AccessToken = accessToken;
                        userAuth.ExpiresAt = expiresAt;
                        userAuth.LastModifyTime = DateTime.Now;
                        _appAuthRepository.Update(userAuth, "AccessToken", "ExpiresAt", "LastModifyTime");

                        appUserEntity = _appUserRepository.IQueryable(r => r.Id == userAuth.UserId && userAuth.DeleteMark == false).FirstOrDefault();
                        appUserEntity.LoginCount += 1;
                        appUserEntity.LastLoginTime = now;
                        appUserEntity.LastLogIp = Net.Ip;
                        appUserEntity.LastModifyTime = now;

                        _appUserRepository.Update(appUserEntity, "LoginCount", "LastLoginTime", "LastLogIp", "LastModifyTime");
                    }

                    var isPersistentCookie = true;
                    FormsAuthentication.SetAuthCookie(qzone.OAuthToken.OpenId, isPersistentCookie);


                    OperatorProvider op = OperatorProvider.Provider;

                    //保存用户信息
                    op.CurrentUser = new OperatorModel
                    {
                        UserId = appUserEntity.Id,
                        IsSystem = false,
                        LoginName = appUserEntity.LoginName,
                        LoginToken = accessToken,
                        LoginTime = now,
                        NickName = appUserEntity.NickName,
                        Avatar = appUserEntity.Avatar
                    };

                    return Redirect(Url.Action("Index", "App"));
                }

            }
            return View();
        }
        #endregion

        public ActionResult ActiveEmail(string t, string u)
        {
            string msg = "";
            if (t.IsNullOrEmpty() || u.IsNullOrEmpty())
            {
                msg = "参数为空，你是要搞事情啊";
            }
            else
            {
                string emailByToken = _iredisHelper.StringGet(t);
                int? userId = _iredisHelper.StringGet<int?>(u);

                if (emailByToken.IsNullOrEmpty() || userId == null)
                {
                    msg = "你这token都已经过期了，我可是给了你1个小时的时间。。。再去拿token吧。";
                }
                else
                {
                    _appUserRepository.Update(_appUserRepository.IQueryable(r => r.Email == emailByToken && r.Id == userId), r => new AppUser
                    {
                        Email = emailByToken,
                        EmailIsValid = true
                    });
                    msg = "您已成功验证该邮箱，你可以用这个邮箱登录系统了！";
                    _iredisHelper.KeyDeleteAsync(t);
                    _iredisHelper.KeyDeleteAsync(u);
                }
            }

            ViewBag.Msg = msg;

            return View();
        }
    }
}