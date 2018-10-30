using System.Web.Optimization;

namespace Mock.luo
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Content/js/jquery-2.1.4-min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-extends").Include(
                "~/Content/js/global.js",
                "~/Content/js/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/js/jquery.validate*"));

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                "~/Content/css/bootstrap.min14ed.css",
                "~/Content/css/font-awesome4.7.min12.css",
                "~/Content/css/animate.min2932.css",
                "~/Content/css/style.min862f.css",
                "~/Content/css/bootstrap-table.min12.css"
                ));
        }
    }
}