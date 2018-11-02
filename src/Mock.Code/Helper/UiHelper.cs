using System.Web;
using RazorEngine;
using RazorEngine.Templating;

namespace Mock.Code.Helper
{
    public class UiHelper
    {
        public static string FormatEmail<T>(T viewModel, string formTemplate)
        {
            string path = HttpContext.Current.Server.MapPath("~/Views/Generic/" + formTemplate + ".cshtml");

            string template = System.IO.File.ReadAllText(path);

            var body = Engine.Razor.RunCompile(template, formTemplate, null, viewModel);

            return body;
        }
    }
}