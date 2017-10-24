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
        public ActionResult GetDataGrid(Pagination pag, string search = "",string EnCode="")
        {
            return Result(_service.GetDataGrid(pag, search, EnCode));
        }
        /// <summary>
        /// 根据主表的编码，获取分表ItemsDetail的关联数据，做为下拉列表数据
        /// </summary>
        /// <param name="Encode"></param>
        /// <returns></returns>
        public ActionResult GetCombobox(string Encode)
        {
            return Result(_service.GetCombobox(Encode));
        }
        /// <summary>
        /// 重写编辑方法，验证编码的唯一性
        /// </summary>
        /// <param name="viewModel">实体</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public override ActionResult Edit(ItemsDetailViewModel viewModel, int id = 0)
        {
            int codeCount = 0;
            if (id == 0)
            {
                codeCount = _service.IQueryable(u => u.ItemCode == viewModel.ItemCode).Count();

            }
            else
            {
                codeCount = _service.IQueryable(u => u.ItemCode == viewModel.ItemCode && u.Id != id).Count();
            }
            if (codeCount > 0)
            {
                return Error("编码不唯一!");
            }
            return base.Edit(viewModel, id);
        }
    }
}