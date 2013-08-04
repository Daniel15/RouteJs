using System.Web.Mvc;
using System.Web.Routing;

namespace RouteJs.Tests.Mvc
{
	public static class MvcRouteTestHelper
	{
		public static RouteCollection RegisterTestRoutes()
		{
			var routes = new RouteCollection();
			routes.MapRoute(
				name: "Hello",
				url: "hello/world",
				defaults: new { controller = "Hello", action = "Index" }
			);
			routes.MapRoute(
				name: "HelloExposed",
				url: "hello/exposed",
				defaults: new { controller = "HelloExposed", action = "Index" }
			);
			routes.MapRoute(
				name: "HelloHidden",
				url: "hello/hidden",
				defaults: new { controller = "HelloHidden", action = "Index" }
			);

			var hiddenArea = new AreaRegistrationContext("HiddenArea", routes);
			hiddenArea.Namespaces.Add("RouteJs.Tests.Mvc.Areas.HiddenArea.*");
			hiddenArea.MapRoute(
				name: "HiddenArea_Hello",
				url: "HiddenArea/Hello",
				defaults: new { controller = "HiddenAreaHello", action = "Index" }
			);
			hiddenArea.MapRoute(
				name: "HiddenArea_default",
				url: "HiddenArea/{controller}/{action}"
			);

			var exposedArea = new AreaRegistrationContext("ExposedArea", routes);
			exposedArea.Namespaces.Add("RouteJs.Tests.Mvc.Areas.ExposedArea.*");
			exposedArea.MapRoute(
				name: "ExposedArea_Hello",
				url: "ExposedArea/Hello",
				defaults: new { controller = "ExposedAreaHello", action = "Index" }
			);
			exposedArea.MapRoute(
				name: "ExposedArea_default",
				url: "ExposedArea/{controller}/{action}"
			);

			var undecoratedArea = new AreaRegistrationContext("UndecoratedArea", routes);
			undecoratedArea.Namespaces.Add("RouteJs.Tests.Mvc.Areas.UndecoratedArea.*");
			undecoratedArea.MapRoute(
				name: "UndecoratedArea_Hello",
				url: "UndecoratedArea/Hello",
				defaults: new { controller = "UndecoratedAreaHello", action = "Index" }
			);
			undecoratedArea.MapRoute(
				name: "UndecoratedArea_default",
				url: "UndecoratedArea/{controller}/{action}"
			);

			return routes;
		}
	}
}
