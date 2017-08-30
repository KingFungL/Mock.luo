using AutoMapper;
using Mock.Data;
using Mock.Data.Models;
using Mock.Luo.Areas.Plat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mock.Luo.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<AppUser, AppUserViewModel>();
                cfg.CreateMap<IDeleteAudited, AppUser>();
                cfg.CreateMap<AppUserViewModel, AppUser>();

                cfg.CreateMap<AppRole, AppRoleViewModel>();
                cfg.CreateMap<IDeleteAudited, AppRole>();
                cfg.CreateMap<AppRoleViewModel, AppRole>();

                cfg.CreateMap<AppMenu, AppMenuViewModel>();
                cfg.CreateMap<IDeleteAudited, AppMenu>();
                cfg.CreateMap<AppMenuViewModel, AppMenu>();

                cfg.CreateMap<Items, ItemsViewModel>();
                cfg.CreateMap<IDeleteAudited, Items>();
                cfg.CreateMap<ItemsViewModel, Items>();

                cfg.CreateMap<ItemsDetail, ItemsDetailViewModel>();
                cfg.CreateMap<IDeleteAudited, ItemsDetail>();
                cfg.CreateMap<ItemsDetailViewModel, ItemsDetail>();
            });
        }
    }
}