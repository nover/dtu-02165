﻿using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace BootstrapSupport
{
    public class BootstrapBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js").Include(
                "~/scripts/jquery-1.*",
                "~/scripts/bootstrap.js",
                "~/scripts/jquery.validate.js",
                "~/scripts/jquery.validate.unobtrusive.js",
                "~/scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js"
                ));

			bundles.Add(new StyleBundle("~/content/css").Include(
				 "~/Content/bootstrap.css",
				 "~/Content/custom.css"
				 ));

            bundles.Add(new StyleBundle("~/content/css-responsive").Include(
                "~/Content/bootstrap-responsive.css"
                ));
        }
    }
}