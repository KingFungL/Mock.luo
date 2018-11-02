using System.Linq;
using System.Web.Mvc;
using Autofac;
using Mock.Code.Web.Tree;
using Mock.Code.Web.TreeGrid;
using Mock.Data.Models;
using Mock.Domain.Interface;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;

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
        /// <summary>
        /// 字典下拉树数据
        /// </summary>
        /// <returns></returns>

        public ActionResult GetTreeJson()
        {
            return Content(_service.GetTreeJson().ComboboxTreeJson());
        }
        /// <summary>
        /// zTree左树结构的数据
        /// </summary>
        /// <returns></returns>

        public ActionResult GetzTreeJson()
        {
            return Result(_service.GetzTreeJson());
        }

        public ActionResult GetTreeGrid()
        {
            return Content(_service.GetTreeGrid().TreeGridJson());
        }

        /// <summary>
        /// 编码需要验证唯一性
        /// </summary>
        /// <param name="viewModel">基础资料类别表</param>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        public override ActionResult Edit(ItemsViewModel viewModel, int id = 0)
        {
            int codeCount = 0;
            if (id == 0)
            {
                codeCount = _service.Queryable(u => u.EnCode == viewModel.EnCode).Count();
            }
            else
            {
                codeCount = _service.Queryable(u => u.EnCode == viewModel.EnCode && u.Id != id).Count();
            }
            if (codeCount > 0)
            {
                return Error("编码不唯一!");
            }
            return base.Edit(viewModel, id);
        }
    }
}