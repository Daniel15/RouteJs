using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using RouteJs.Mvc4;

namespace RouteJs.Tests
{
	[TestFixture]
	public class MvcDefaultsProcessorTests
	{
		[Test]
		public void HandlesOneOptionalParameter()
		{
			var routes = new RouteCollection();
			routes.MapRoute(
				name: "Hello",
				url: "hello/world/{id}",
				defaults: new { controller = "Hello", action = "Index", id = UrlParameter.Optional }
			);

			var processor = new DefaultsProcessor();
			var routeInfo = new RouteInfo();
			processor.ProcessDefaults((Route)routes["Hello"], routeInfo);

			Assert.AreEqual(2, routeInfo.Defaults.Count);
			Assert.AreEqual("Hello", routeInfo.Defaults["controller"]);
			Assert.AreEqual("Index", routeInfo.Defaults["action"]);
			Assert.AreEqual(1, routeInfo.Optional.Count);
			Assert.AreEqual("id", routeInfo.Optional[0]);
		}

		[Test]
		public void HandlesTwoOptionalParameters()
		{
			var routes = new RouteCollection();
			routes.MapRoute(
				name: "Hello",
				url: "hello/world/{first}/{second}",
				defaults: new { controller = "Hello", action = "Index", first = UrlParameter.Optional, second = UrlParameter.Optional }
			);

			var processor = new DefaultsProcessor();
			var routeInfo = new RouteInfo();
			processor.ProcessDefaults((Route)routes["Hello"], routeInfo);

			Assert.AreEqual(2, routeInfo.Defaults.Count);
			Assert.AreEqual(2, routeInfo.Optional.Count);
			Assert.AreEqual("first", routeInfo.Optional[0]);
			Assert.AreEqual("second", routeInfo.Optional[1]);
		}
	}
}
