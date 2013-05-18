using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RouteJs.Samples.Mvc4
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Hello",
				url: "hello/world",
				defaults: new { controller = "Hello", action = "HelloWorld" }
			);

			routes.MapRoute(
				name: "Hello2",
				url: "hello/mvc/{message}",
				defaults: new { controller = "Hello", action = "HelloWorld2" }
			);

			routes.MapPageRoute("TestPageRoute", "hello/webforms", "~/HelloWebForms.aspx");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}