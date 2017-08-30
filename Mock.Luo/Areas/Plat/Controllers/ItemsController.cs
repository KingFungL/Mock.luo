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
    public class ItemsController : CrudController<Items, ItemsViewModel>
    {
        // GET: Plat/Items

        private readonly IItemsRepository _service;
        public ItemsController(IItemsRepository service, IComponentContext container) : base(container)
        {
            this._service = service;
        }

        public ActionResult GetTreeJson()
        {
            return Content(_service.GetTreeJson().TreeSelectJson());
        }

        public ActionResult GetzTreeJson()
        {
            return Result(_service.GetzTreeJson());
        }

        public ActionResult GetTreeGrid()
        {
            return Content(_service.GetTreeGrid().TreeGridJson());
        }
    }
}