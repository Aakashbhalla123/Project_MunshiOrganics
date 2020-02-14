using System.Web;
using System.Web.Optimization;

namespace AOneNutsWebWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/MunshiJS").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/CreateInputRowTable.js",
                        "~/Scripts/EntryGridControl.js",
                        "~/Scripts/addNewSelect2.js",
                        "~/Scripts/select2.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.number.min.js",
                        "~/Scripts/jquery.Guid.js"
                        //"~/Scripts/daterangepicker.js",
                        //"~/Scripts/bootstrap.js",
                        //"~/Scripts/Mask.js",
                        //"~/Scripts/fieldChooser.js",

                        //"~/Scripts/jquery.msgbox.min.js",
                        //"~/Scripts/formValidation.min.js",
                        //"~/Scripts/formValidation-Bootstrap.min.js"
                        ));

            bundles.Add(new StyleBundle("~/bundles/MunshiCSS").Include(
                       "~/Content/select2-bootstrap.css",
                        //"~/Content/jquery-ui.css",
                        "~/Content/bootstrap.css",
                        //"~/Content/jquery-msgbox.css",
                        //"~/Content/daterangepicker-bs3.css",
                        //"~/Content/formValidation.min.css",
                        "~/Content/select2.css"
                        ));
        }
    }
}
