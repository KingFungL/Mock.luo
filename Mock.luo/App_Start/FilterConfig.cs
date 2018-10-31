using System.Web.Mvc;
using Mock.Luo.Generic.Filters;

namespace Mock.Luo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandlerLoginAttribute());
        }
    }
}