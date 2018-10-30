using System.Web.Mvc;

namespace Mock.luo.Areas.Plat
{
    public class PlatAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Plat";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Plat_default",
                "Plat/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}