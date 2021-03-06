﻿using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace BootstrapSupport
{
	public class BootstrapBundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/js-jquery").Include(
				"~/Scripts/jquery-1.*",
				"~/Scripts/underscore.js"));

			bundles.Add(new ScriptBundle("~/js").Include(
				"~/Scripts/jquery-1.*",
				"~/Scripts/bootstrap.js",
				"~/Scripts/jquery.validate.js",
				"~/scripts/jquery.validate.unobtrusive.js",
				"~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js"
				));

			bundles.Add(new StyleBundle("~/content/css").Include(
				"~/Content/bootstrap.css",
				"~/Content/mystyles.css"
				));
			bundles.Add(new StyleBundle("~/content/css-responsive").Include(
				"~/Content/bootstrap-responsive.css"
				));

			bundles.Add(new StyleBundle("~/bundles/modernizr").Include(
				"~/Content/modernizr-*"
				));
		}
	}
}