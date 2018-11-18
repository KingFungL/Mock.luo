using System;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using AutoMapper;
using Mock.Code.Helper;
using Mock.Code.Json;
using Mock.Data.AppModel;
using Mock.Data.Infrastructure;
using Mock.Data.Repository;
using Mock.Luo.Generic.Filters;

namespace Mock.Luo.Controllers
{
    public class CrudController<TEntityModel, TViewModel> : BaseController where TEntityModel : class, new() where TViewModel : class, new()
    {

        private readonly IRepositoryBase<TEntityModel> _ibase;
        protected readonly IRedisHelper RedisHelper;
        public CrudController(IComponentContext container)
        {
            _ibase = container.Resolve<IRepositoryBase<TEntityModel>>();
            this.RedisHelper = container.Resolve<IRedisHelper>();
        }
        //[HandlerAuthorize]
        public virtual ActionResult Form(int id)
        {
            ViewBag.ViewModel = this.GetFormJson(id);
            return View();
        }

        #region 根据ID获取数据
        /// <summary>
        /// 此时表单中没有默认值，就可以用这个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetFormJson(int id = 0)
        {
            TViewModel viewModel = new TViewModel { };
            if (id != 0)
            {
                TEntityModel entity = _ibase.FindEntity(id);
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
        /// <param name="viewModel">实体对象</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        //[HandlerAuthorize]
        public virtual ActionResult Edit(TViewModel viewModel, int id = 0)
        {
            if (!ModelState.IsValid)
            {
                return Error(ModelState.Values.FirstOrDefault(u => u.Errors.Count > 0)?.Errors[0].ErrorMessage);
            }

            var userid = OperatorProvider.Provider.CurrentUser.UserId;
            //新增
            if (id == 0)
            {
                TEntityModel entity = Mapper.Map<TViewModel, TEntityModel>(viewModel);

                if (entity is ICreationAudited d)
                {
                    d.CreatorTime = DateTime.Now;
                    d.CreatorUserId = userid;
                    d.DeleteMark = false;
                    TEntityModel tEntityModel = d as TEntityModel;
                   

                    if (_ibase.Insert(tEntityModel) > 0)
                    {
                        return Success("新增成功");
                    }
                }
                return Error("新增失败");
            }
            else
            {
                var tEntityModel = _ibase.FindEntity(id);

                if (tEntityModel == null)
                    return Error($"Id为{id}未找到任何类型为{viewModel.GetType().Name}的实体对象");

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
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAuthorize]
        public ActionResult Delete(int id)
        {
            var codetableEntity = _ibase.FindEntity(id);

            if (codetableEntity == null)
                return Error($"Id为{id}未找到任何类型为{typeof(TEntityModel).Name}的实体对象");
            IDeleteAudited deleteAudited = new DeleteAudited { Id = id, DeleteMark = true, DeleteTime = DateTime.Now, DeleteUserId = OperatorProvider.Provider.CurrentUser.UserId };

            TEntityModel entity = Mapper.Map(deleteAudited, codetableEntity);

            if (_ibase.Update(entity, "DeleteMark", "DeleteUserId", "DeleteTime") > 0)
            {
                return Success("删除成功！");
            }
            return Error("删除失败！");
        }

    }
}
