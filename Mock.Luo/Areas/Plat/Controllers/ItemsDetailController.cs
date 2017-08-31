using Autofac;
using Mock.Code;
using Mock.Data;
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
    public class ItemsDetailController : CrudController<ItemsDetail, ItemsDetailViewModel>
    {
        // GET: Plat/ItemsDetail

        private readonly IItemsDetailRepository _service;
        public ItemsDetailController(IItemsDetailRepository service, IComponentContext container) : base(container)
        {
            this._service = service;
        }
        public ActionResult GetDataGrid(Pagination pag, string ItemName = "", string EnCode = "")
        {
            return Result(_service.GetDataGrid(pag, ItemName, EnCode));
        }

        public ActionResult GetCombobox(string Encode)
        {
            return Result(_service.GetCombobox(Encode));
        }

        public override ActionResult Edit(ItemsDetailViewModel viewModel, int id=0)
        {
            return base.Edit(viewModel, id);
        }
    }
}