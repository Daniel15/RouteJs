using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;

namespace RouteJs.Tests
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
	}
}
