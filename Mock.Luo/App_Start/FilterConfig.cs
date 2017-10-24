using Mock.Luo.Generic.Filters;
using System.Web.Mvc;

namespace Mock.Luo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandlerErrorAttribute());
            filters.Add(new HandlerLoginAttribute());
        }
    }
}