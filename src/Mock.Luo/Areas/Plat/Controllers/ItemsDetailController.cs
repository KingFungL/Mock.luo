using System.Linq;
using System.Web.Mvc;
using Autofac;
using Mock.Code.Helper;
using Mock.Code.Web;
using Mock.Data.Models;
using Mock.Domain.Interface;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;

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
        public ActionResult GetDataGrid(PageDto pag, string search = "",string enCode="")
        {
            return Result(_service.GetDataGrid(pag, search, enCode));
        }
        /// <summary>
        /// 根据主表的编码，获取分表ItemsDetail的关联数据，做为下拉列表数据
        /// </summary>
        /// <param name="encode"></param>
        /// <returns></returns>
        public ActionResult GetCombobox(string encode)
        {
            return CamelCaseJson(_service.GetCombobox(encode));
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
                codeCount = _service.Queryable(u => u.ItemCode == viewModel.ItemCode).Count();
            }
            else
            {
                codeCount = _service.Queryable(u => u.ItemCode == viewModel.ItemCode && u.Id != id).Count();
            }
            if (codeCount > 0)
            {
                return Error("编码不唯一!");
            }
            RedisHelper.KeyDeleteAsync(string.Format(ConstHelper.ItemsDetailAll, "GetCombobox-"+EnCode.FTypeCode.ToString()));
            RedisHelper.KeyDeleteAsync(string.Format(ConstHelper.ItemsDetailAll, "GetCombobox-" + EnCode.Tag.ToString()));
            RedisHelper.KeyDeleteAsync(string.Format(ConstHelper.Article, "category"));

            return base.Edit(viewModel, id);
        }
    }
}