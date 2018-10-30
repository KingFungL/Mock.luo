using System.Web.Mvc;
using Mock.luo.Generic.Filters;

namespace Mock.luo
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