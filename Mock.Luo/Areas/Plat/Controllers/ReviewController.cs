using Mock.Domain;
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

        private readonly IReviewRepository _service;
        public ReviewController(IReviewRepository service)
        {
            this._service = service;
        }
        public ActionResult GetRecentReView()
        {
           var rows= _service.GetRecentReview(10);
            return Result(rows);
        }
    }
}