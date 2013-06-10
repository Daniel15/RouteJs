using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using NUnit.Framework;
using System.Web.Mvc;

namespace RouteJs.Tests
{
	[TestFixture]
	public class T4MvcRouteTests : MvcTestBase
	{
		private List<RouteInfo> GetSimpleRoute()
		{
			var routes = new RouteCollection();
			routes.MapRoute(
				name: "Hello",
				url: "hello/world",
				defaults: MVC.Hello.HelloWorld()
			);
			var routejs = CreateRouteJs(routes);

			return routejs.GetRoutes().ToList();
		}

		[Test]
		public void HandlesSimpleT4MvcRoute()
		{
			var result = GetSimpleRoute();

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("hello/world", result[0].Url);
			Assert.AreEqual("Hello", result[0].Defaults["controller"]);
			Assert.AreEqual("HelloWorld", result[0].Defaults["action"]);
		}

		[Test]
		public void ShouldNotSerializeRouteValueDictionaryInDefaults()
		{
			var result = GetSimpleRoute()[0];
			Assert.False(result.Defaults.ContainsKey("RouteValueDictionary"));
		}

		public class T4MvcResult : IT4MVCActionResult
		{
			public string Action { get; set; }
			public string Controller { get; set; }
			public RouteValueDictionary RouteValueDictionary { get; set; }
			public string Protocol { get; set; }
		}
	}
}
