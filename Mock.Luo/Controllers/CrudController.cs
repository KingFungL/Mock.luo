using Autofac;
using AutoMapper;
using Mock.Code;
using Mock.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Controllers
{
    public class CrudController<TEntityModel, TViewModel> : BaseController where TEntityModel : class, new() where TViewModel : class, new()
    {

        private readonly IRepositoryBase<TEntityModel> _ibase;
        public CrudController(IComponentContext container)
        {
            _ibase = container.Resolve<IRepositoryBase<TEntityModel>>();
        }

        public virtual ActionResult Form(int Id)
        {
            ViewBag.ViewModel = this.GetFormJson(Id);
            return View();
        }

        #region 根据ID获取数据
        /// <summary>
        /// 此时表单中没有默认值，就可以用这个
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetFormJson(int Id = 0)
        {
            TViewModel viewModel = new TViewModel { };
            if (Id != 0)
            {
                TEntityModel entity = _ibase.FindEntity(Id);
                if (entity != null)
                {

                    viewModel = Mapper.Map<TEntityModel, TViewModel>(entity);
                }
            }

            return viewModel.ToJson();
        }
        #endregion


        #region Edit 统一的新增，编辑方法，每次用一次拆箱，装箱，不知道会不会影响速度
        /// <summary>
        /// author:luozQ
        /// function:Edit 统一的新增，编辑方法，每次用一次拆箱，装箱，不知道会不会影响速度
        /// </summary>
        /// <param name="Id">主键</param>
        /// <param name="viewModel">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Edit(TViewModel viewModel, int Id = 0)
        {
            var userid = OperatorProvider.Provider.GetCurrent().UserId;
            //新增
            if (Id == 0)
            {
                TEntityModel entity = Mapper.Map<TViewModel, TEntityModel>(viewModel);

                var d = entity as ICreationAudited;
                if (d != null)
                {
                    d.CreatorTime = DateTime.Now;
                    d.CreatorUserId = userid;
                    d.DeleteMark = false;
                    TEntityModel tEntityModel = d as TEntityModel;

                    if (tEntityModel != null && _ibase.Insert(tEntityModel) > 0)
                    {
                        return Success("新增成功");
                    }
                }
                return Error("新增失败");
            }
            else
            {
                var tEntityModel = _ibase.FindEntity(Id);

                if (tEntityModel == null)
                    return Error($"Id为{Id}未找到任何类型为{viewModel.GetType().Name}的实体对象");

                TEntityModel entity = Mapper.Map(viewModel, tEntityModel);

                var d = entity as IModificationAudited;
                if (d != null)
                {
                    d.LastModifyTime = DateTime.Now;
                    d.LastModifyUserId = userid;
                    TEntityModel tUpdateEntityModel = d as TEntityModel;

                    if (tUpdateEntityModel != null && _ibase.Update(tUpdateEntityModel) > 0)
                    {
                        return Success("编辑成功");
                    }
                }
                return Error("编辑失败");
            }
        }
        #endregion


        /// <summary>
        /// 统一的删除功能，其类要继承IDeleteAduited这个类才行。
        /// </summary>
        /// <param name="Id">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var codetableEntity = _ibase.FindEntity(Id);

            if (codetableEntity == null)
                return Error($"Id为{Id}未找到任何类型为{codetableEntity.GetType().Name}的实体对象");
            IDeleteAudited deleteAudited = new DeleteAudited { Id = Id, DeleteMark = true, DeleteTime = DateTime.Now, DeleteUserId = OperatorProvider.Provider.GetCurrent().UserId };

            TEntityModel entity = Mapper.Map(deleteAudited, codetableEntity);

            if (_ibase.Update(entity, "DeleteMark", "DeleteUserId", "DeleteTime") > 0)
            {
                return Success("删除成功！");
            }
            return Error("删除失败！");
        }
    }
}
