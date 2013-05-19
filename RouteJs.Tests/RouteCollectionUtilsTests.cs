using System.Web.Routing;
using NUnit.Framework;
using System.Web.Mvc;

namespace RouteJs.Tests
{
	[TestFixture]
	public class RouteCollectionUtilsTests
	{
		[Test]
		public void GetRoutesWorksInCurrentEnvironment()
		{
			var routeCollection = new RouteCollection();
			routeCollection.MapRoute("MvcTest", "mvctest", new { controller = "Home", action = "Index" });

			var routes = routeCollection.GetRoutes();

			Assert.IsNotNull(routes);
			Assert.AreEqual(1, routes.Count);
			Assert.IsInstanceOf<Route>(routes[0]);
			Assert.AreEqual("mvctest", ((Route)routes[0]).Url);
		}

		[Test]
		public void GetRoutesReturnsRoutesInCorrectOrder()
		{
			var routeCollection = new RouteCollection();
			routeCollection.MapRoute("bbbb", "bbbb", new { controller = "Home", action = "Index" });
			routeCollection.MapRoute("aaaa", "aaaa", new { controller = "Home", action = "Index" });

			var routes = routeCollection.GetRoutes();
			Assert.AreEqual("bbbb", ((Route)routes[0]).Url);
			Assert.AreEqual("aaaa", ((Route)routes[1]).Url);
		}
	}
}
