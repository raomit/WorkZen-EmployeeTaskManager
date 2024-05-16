using System.Security.Cryptography;
using System.Web;
using System.Web.Optimization;
using System.Web.UI.WebControls;

namespace WorkZen
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

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new Bundle("~/Content/vendorcss").Include("~/Content/assets/vendor/mdi/css/materialdesignicons.min.css",
                "~/Content/assets/vendors/ti-icons/css/themify-icons.css",
                "~/Content/assets/vendors/css/vendor.bundle.base.css",
                "~/Content/assets/vendors/font-awesome/css/font-awesome.min.css",
                "~/Content/assets/vendors/mdi/css/materialdesignicons.min.css",
                "~/Content/assets/vendors/select2/select2.min.css",
                "~/Content/assets/vendors/select2-bootstrap-theme/select2-bootstrap.min.css",
                "~/Content/assets/css/style.css"
              ));   

            bundles.Add(new StyleBundle("~/Content/layoutCss").Include("~/Content/assets/css/style.css", "~/Content/assets/Datatable/datatables.css", "~/Content/assets/Datatable/datatables.min.css"));

            bundles.Add(new Bundle("~/bundles/vendorjs").Include("~/Content/assets/vendors/js/vendor.bundle.base.js",
                "~/Content/assets/vendors/select2/select2.min.js",
                "~/Content/assets/vendors/typeahead.js/typeahead.bundle.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Content/assets/js/off-canvas.js",
                "~/Content/assets/js/misc.js",
                "~/Content/assets/js/settings.js",
                "~/Content/assets/js/todolist.js",
                "~/Content/assets/js/jquery.cookie.js",
                "~/Content/assets/js/file-upload.js",
                "~/Content/assets/js/typeahead.js",
                "~/Content/assets/js/select2.js",
                "~/Content/assets/js/main.js"
                ));

            bundles.Add(new Bundle("~/Content/DatatblesJs").Include("~/Content/assets/DataTable/datatables.js", "~/Content/assets/DataTable/datatables.min.js"));
            
        }
    }
}
