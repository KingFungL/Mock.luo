using Mock.Luo.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Mock.Luo.Generic.Helper
{
    public class UiHelper
    {
        public static string FormatEmail(EmailViewModel viewModel, string FormTemplate)
        {
            string path = HttpContext.Current.Server.MapPath("~/Views/Generic/" + FormTemplate + ".cshtml");

            string template = System.IO.File.ReadAllText(path);

            var body = Engine.Razor.RunCompile(template, FormTemplate, null, viewModel);

            return body;
        }
    }
}