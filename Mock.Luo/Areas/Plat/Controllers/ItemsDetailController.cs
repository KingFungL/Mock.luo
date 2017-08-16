using Mock.Domain;
using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class ItemsDetailController : BaseController
    {
        // GET: Plat/ItemsDetail

        private readonly IItemsDetailRepositroy _service;
        public ItemsDetailController(IItemsDetailRepositroy service)
        {
            this._service = service;
        }


        public ActionResult GetItemDetailsFCode()
        {
            return Result(_service.GetItemDetailsFCode("category"));
        }

    }
}