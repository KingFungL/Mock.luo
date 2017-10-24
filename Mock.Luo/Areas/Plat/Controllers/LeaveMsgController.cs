using Mock.Data.Models;
using Mock.Luo.Areas.Plat.Models;
using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Mock.Domain;
using Mock.Code;

namespace Mock.Luo.Areas.Plat.Controllers
{
    public class LeaveMsgController : CrudController<LeaveMsg, LeaveMsgViewModel>
    {
        // GET: Plat/LeaveMsg

      private readonly   ILeaveMsgRepository _service;
        public LeaveMsgController(ILeaveMsgRepository service,IComponentContext container) : base(container)
        {
            this._service = service;
        }

        public ActionResult GetDataGrid(Pagination pag,string search = "")
        {
            return Result(_service.GetDataGrid(pag, search));
        }

        /// <summary>
        /// 审核、拉黑留言
        /// </summary>
        /// <param name="IsAduit"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Aduit(bool IsAduit, int Id)
        {
            LeaveMsg entity = new LeaveMsg { Id = Id, IsAduit = IsAduit };
            entity.Modify(Id);
            _service.Update(entity, "IsAduit", "LastModifyUserId", "LastModifyTime");
            return Success(IsAduit ? "审核成功！" : "拉黑成功！");
        }


    }
}