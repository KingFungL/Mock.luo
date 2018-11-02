using AutoMapper;
using Mock.Data;
using Mock.Data.Infrastructure;
using Mock.Data.Models;
using Mock.Luo.Areas.Plat.Models;

namespace Mock.Luo
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

                cfg.CreateMap<AppModule, AppModuleViewModel>();
                cfg.CreateMap<IDeleteAudited, AppModule>();
                cfg.CreateMap<AppModuleViewModel, AppModule>();

                cfg.CreateMap<Items, ItemsViewModel>();
                cfg.CreateMap<IDeleteAudited, Items>();
                cfg.CreateMap<ItemsViewModel, Items>();

                cfg.CreateMap<ItemsDetail, ItemsDetailViewModel>();
                cfg.CreateMap<IDeleteAudited, ItemsDetail>();
                cfg.CreateMap<ItemsDetailViewModel, ItemsDetail>();

                cfg.CreateMap<Article, ArticleViewModel>();
                cfg.CreateMap<IDeleteAudited, Article>();
                cfg.CreateMap<ArticleViewModel, Article>();

                cfg.CreateMap<GuestBook, GuestBookViewModel>();
                cfg.CreateMap<IDeleteAudited, GuestBook>();
                cfg.CreateMap<GuestBookViewModel, GuestBook>();
            });
        }
    }
}