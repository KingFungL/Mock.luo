using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Mock.Data.Models;

namespace Mock.Luo
{

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            AutofacConfig.Register();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MockDbContext, Mock.Data.Migrations.Configuration>());
            var dbMigrator = new DbMigrator(new Mock.Data.Migrations.Configuration());
            dbMigrator.Update();
        }
    }
}