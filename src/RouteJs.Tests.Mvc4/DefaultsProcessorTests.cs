using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using RouteJs.Mvc;

namespace RouteJs.Tests.Mvc
{
	[TestFixture]
	public class DefaultsProcessorTests
	{
		[Test]
		public void HandlesRouteWithoutDefaults()
		{
			var route = new Route("hello/world", null, null);

			var routeInfo = new RouteInfo();
			var defaultsProcessor = new DefaultsProcessor();
			defaultsProcessor.ProcessDefaults(route, routeInfo);

			Assert.AreEqual(0, routeInfo.Defaults.Count);
			Assert.AreEqual(0, routeInfo.Optional.Count);
		}

		[Test]
		public void HandlesMvcControllerAndAction()
		{
			var routes = new RouteCollection();
			routes.MapRoute(
				name: "Hello",
				url: "hello/world",
				defaults: new { controller = "Hello", action = "HelloWorld" }
			);

			var routeInfo = new RouteInfo();
			var defaultsProcessor = new DefaultsProcessor();
			defaultsProcessor.ProcessDefaults((Route)routes["Hello"], routeInfo);

			Assert.AreEqual("Hello", routeInfo.Defaults["controller"]);
			Assert.AreEqual("HelloWorld", routeInfo.Defaults["action"]);
		}

		[Test]
		public void HandlesOptionalParameters()
		{
			var routes = new RouteCollection();
			routes.MapRoute(
				name: "Hello",
				url: "hello/world/{id}",
				defaults: new { controller = "Hello", action = "HelloWorld", id = UrlParameter.Optional }
			);

			var routeInfo = new RouteInfo();
			var defaultsProcessor = new DefaultsProcessor();
			defaultsProcessor.ProcessDefaults((Route)routes["Hello"], routeInfo);

			Assert.IsTrue(routeInfo.Optional.Contains("id"));
		}

		[Test]
		public void HandlesAreas()
		{
			var routes = new RouteCollection();
			var areaReg = new AreaRegistrationContext("Foobar", routes);
			areaReg.MapRoute(
				name: "Hello",
				url: "hello/world",
				defaults: new { controller = "Hello", action = "HelloWorld" }
			);

			var routeInfo = new RouteInfo();
			var defaultsProcessor = new DefaultsProcessor();
			defaultsProcessor.ProcessDefaults((Route)routes["Hello"], routeInfo);

			Assert.AreEqual("Foobar", routeInfo.Defaults["area"]);
		}
	}
}
