using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Web;

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

        public static string GetRandomUserUrl()
        {
            return "/Content/user/" + new Random(DateTime.Now.Second).Next(1, 361) + ".png";
        }
    }
}