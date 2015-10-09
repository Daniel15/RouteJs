using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;

namespace RouteJs.Tests.Mvc
{
	[TestFixture]
	public class MvcRouteTests : MvcTestBase
	{
		[Test]
		public void HandlesSimpleMvcRoute()
		{
			var routes = new RouteCollection();
			routes.MapRoute(
				name: "Hello",
				url: "hello/world",
				defaults: new { controller = "Hello", action = "HelloWorld" }
			);
			var routeJs = CreateRouteJs(routes);

			var result = routeJs.GetRoutes().ToList();

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("hello/world", result[0].Url);
			Assert.AreEqual("Hello", result[0].Defaults["controller"]);
			Assert.AreEqual("HelloWorld", result[0].Defaults["action"]);
		}

		[Test]
		public void HandlesAreaRoute()
		{
			var routes = new RouteCollection();
			var areaContext = new AreaRegistrationContext("TestArea", routes);
			areaContext.MapRoute(
				name: "TestIndex",
				url: "test/hello/world",
				defaults: new { controller = "Hello", action = "HelloWorld" }
			);
			var routeJs = CreateRouteJs(routes);

			var result = routeJs.GetRoutes().ToList();

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("test/hello/world", result[0].Url);
			Assert.AreEqual("Hello", result[0].Defaults["controller"]);
			Assert.AreEqual("HelloWorld", result[0].Defaults["action"]);
			Assert.AreEqual("TestArea", result[0].Defaults["area"]);
		}

		[Test]
		public void HandlesAreaWithDefaultArea()
		{
			var routes = new RouteCollection();
			var areaContext = new AreaRegistrationContext("TestArea", routes);
			areaContext.MapRoute(
				name: "TestIndex",
				url: "test/hello/world",
				// Area routes don't actually need the "area" param to be specified here.
				// However, we're just testing to make sure this doesn't throw an exception.
				defaults: new { controller = "Hello", action = "HelloWorld", area = "TestArea" }
			);
			var routeJs = CreateRouteJs(routes);

			var result = routeJs.GetRoutes().ToList();

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("test/hello/world", result[0].Url);
			Assert.AreEqual("Hello", result[0].Defaults["controller"]);
			Assert.AreEqual("HelloWorld", result[0].Defaults["action"]);
			Assert.AreEqual("TestArea", result[0].Defaults["area"]);
		}

		[Test]
		public void HandlesLowerCaseUrls()
		{
			var routes = new RouteCollection();
			routes.MapRoute(
				name: "Hello",
				url: "Hello/World",
				defaults: new { controller = "Hello", action = "Hello-World" }
			);
			var routeJs = CreateRouteJs(routes);
			routeJs.InstanceLowerCaseUrls = true;

			var result = routeJs.GetRoutes().ToList();

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("hello/world", result[0].Url);
			Assert.AreEqual("hello", result[0].Defaults["controller"]);
			Assert.AreEqual("hello-world", result[0].Defaults["action"]);
		}
	}
}
