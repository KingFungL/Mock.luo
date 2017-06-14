using System.Web;
using System.Web.Optimization;

namespace Mock.Luo
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-extends").Include(
                "~/Scripts/jquery.extend.js",
                "~/Scripts/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/EasyUI151").Include(
                "~/EasyUI151-Insdep-Them/jquery.easyui-1.5.1-min.js",
                "~/EasyUI151-Insdep-Them/themes/insdep/jquery.insdep-extend-min.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/EasyUI151/themesth/insdep/css").Include(
                        "~/EasyUI151-Insdep-Them/themes/insdep/easyui.css",
                        "~/EasyUI151-Insdep-Them/themes/insdep/easyui_animation.css",
                        "~/EasyUI151-Insdep-Them/themes/insdep/easyui_plus.css",
                        "~/EasyUI151-Insdep-Them/themes/insdep/insdep_theme_default.css",
                        "~/EasyUI151-Insdep-Them/themes/insdep/icon.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                "~/Content/css/bootstrap.min14ed.css",
                "~/Content/css/font-awesome.min93e3.css",
                "~/Content/css/animate.min.css",
                "~/Content/css/style.min862f.css"
                ));
        }
    }
}