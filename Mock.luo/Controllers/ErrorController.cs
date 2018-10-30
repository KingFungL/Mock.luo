using System.Web.Mvc;
using Mock.Code.Attribute;

namespace Mock.luo.Controllers
{
    [Skip]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HaveLogin()
        {
            return View();
        }

        public ActionResult ReturnToLogin()
        {
            return View();
        }

        public ActionResult E500()
        {
            return View();
        }

        public ActionResult E404()
        {
            return View();
        }
        public ActionResult LockScreen()
        {
            return View();
        }

    }
}