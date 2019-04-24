using System.Web;
using System.Web.Optimization;

namespace EvolutyzCorner.UI.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     // "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/EvolutyzCSS").Include(
                "~/Content/css/bootstrap-theme.css",
                //"~/Content/css/bootstrap.css",
                "~/Content/css/bootstrap-multiselect.css",
                "~/Content/css/family-tree-styles.css",
                "~/Content/css/ionicons.css",
                "~/Content/assets/fonts/jquery.filer-icons/jquery-filer.css",
                "~/Content/css/jquery.filer.css",
                "~/Content/css/themes/jquery.filer-dragdropbox-theme.css",
                "~/Content/css/feedback.css",
                "~/Content/css/video-js.css",
                "~/Scripts/js/VideoPlayer/sublime-video-js.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css",
                "~/plugins/datetimepicker/jquery.datetimepicker.css"));
        }
    }
}
