using Mock.Code.Attribute;
using Mock.Code.Net;
using Mock.Data.Models;
using Mock.Domain.Interface;
using Mock.Luo.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

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

        [Skip]
        //用户点赞
        public ActionResult Edit(PointArticle entry)
        {
            entry.AddTime = DateTime.Now;
            entry.IP = Net.Ip;
            entry.Browser = Net.Browser;
            entry.System = Net.GetOsNameByUserAgent(Request.UserAgent);
            entry.Agent = Request.UserAgent;
            int pointCount = 0;
            var currentUser = Op.CurrentUser;
            if (currentUser != null)
            {
                entry.UserId = (int)currentUser.UserId;
                entry.LoginName = currentUser.LoginName;
                entry.Email = currentUser.Email;
                pointCount = _service.Queryable(u => u.UserId == entry.UserId && u.AId == entry.AId).Count();
            }
            else
            {
                pointCount = _service.Queryable(r => r.IP == entry.IP && r.AId == entry.AId).Count();
            }
            //点当前文章已经被点赞时，用户再次点赞，
            if (pointCount > 0)
            {
                return Error("你已经点过赞了！");
            }
            else
            {
                _service.Insert(entry);
                int pointQuantity = _articleRepository.Queryable(r => r.Id == entry.AId).Select(r => r.PointQuantity).FirstOrDefault();

                _articleRepository.Update(new Article { Id = entry.AId, PointQuantity = pointQuantity + 1 }, "PointQuantity");
            }

            return Success("成功点赞");
        }

    }
}