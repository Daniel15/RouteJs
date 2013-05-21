using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using RouteJs.Mvc4;

namespace RouteJs.Tests
{
	[TestFixture]
	public class MvcRouteFilterTests
	{
		private RouteCollection _routes;

		[SetUp]
		public void BeforeEach()
		{
			_routes = new RouteCollection();
			_routes.MapRoute(
				name: "Hello",
				url: "hello/world",
				defaults: new { controller = "Hello", action = "Index" }
			);
			_routes.MapRoute(
				name: "HelloExposed",
				url: "hello/exposed",
				defaults: new { controller = "HelloExposed", action = "Index" }
			);
			_routes.MapRoute(
				name: "HelloHidden",
				url: "hello/hidden",
				defaults: new { controller = "HelloHidden", action = "Index" }
			);
		}

		[Test]
		public void ControllersCanBeHiddenViaAttribute()
		{
			var config = new Mock<IConfiguration>();
			var routeFilter = new MvcRouteFilter(config.Object);

			var result = routeFilter.AllowRoute(_routes["HelloHidden"]);
			Assert.IsFalse(result);
		}

		[Test]
		public void ControllersCanBeExposedViaAttribute()
		{
			var config = new Mock<IConfiguration>();
			var routeFilter = new MvcRouteFilter(config.Object);

			var result = routeFilter.AllowRoute(_routes["HelloExposed"]);
			Assert.IsTrue(result);
		}

		[Test]
		public void RoutesCanBeExposedByDefault()
		{
			var config = new Mock<IConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(true);
			var routeFilter = new MvcRouteFilter(config.Object);

			var result = routeFilter.AllowRoute(_routes["Hello"]);
			Assert.IsTrue(result);
		}

		[Test]
		public void HiddenControllersAreHiddenWhenRoutesAreExposedByDefault()
		{
			var config = new Mock<IConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(true);
			var routeFilter = new MvcRouteFilter(config.Object);

			var result = routeFilter.AllowRoute(_routes["HelloHidden"]);
			Assert.IsFalse(result);
		}

		[Test]
		public void RoutesCanBeHiddenByDefault()
		{
			var config = new Mock<IConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(false);
			var routeFilter = new MvcRouteFilter(config.Object);

			var result = routeFilter.AllowRoute(_routes["Hello"]);
			Assert.IsFalse(result);
		}

		[Test]
		public void ExposedControllersAreExposedWhenRoutesAreHiddenByDefault()
		{
			var config = new Mock<IConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(false);
			var routeFilter = new MvcRouteFilter(config.Object);

			var result = routeFilter.AllowRoute(_routes["HelloExposed"]);
			Assert.IsTrue(result);
		}
	}
}
