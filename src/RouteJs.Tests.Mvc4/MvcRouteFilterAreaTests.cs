using System.Web.Routing;
using Moq;
using NUnit.Framework;
using RouteJs.Mvc;

namespace RouteJs.Tests.Mvc.MvcRouteFilterAreaTests
{
	public class AreaTestBase
	{
		protected RouteCollection _routes;

		[SetUp]
		public void BeforeEach()
		{
			_routes = MvcRouteTestHelper.RegisterTestRoutes();
		}
	}

	[TestFixture]
	public class UndecoratedArea : AreaTestBase
	{
		[Test]
		public void RouteCanBeExposedByDefault()
		{
			var config = new Mock<IRouteJsConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(true);
			var routeFilter = new MvcRouteFilter(config.Object, _routes);

			var areaResult = routeFilter.AllowRoute(_routes["UndecoratedArea_Hello"]);
			Assert.IsTrue(areaResult);
		}

		[Test]
		public void RouteCanBeHiddenByDefault()
		{
			var config = new Mock<IRouteJsConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(false);
			var routeFilter = new MvcRouteFilter(config.Object, _routes);

			var areaResult = routeFilter.AllowRoute(_routes["UndecoratedArea_Hello"]);
			Assert.IsFalse(areaResult);
		}

		[Test]
		public void DefaultRouteIsExposedIfControllerIsExposedByDefault()
		{
			var config = new Mock<IRouteJsConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(true);
			var routeFilter = new MvcRouteFilter(config.Object, _routes);

			var areaResult = routeFilter.AllowRoute(_routes["UndecoratedArea_default"]);
			Assert.IsTrue(areaResult);
		}

		[Test]
		public void DefaultRouteIsHiddenIfControllerIsHiddenByDefault()
		{
			var config = new Mock<IRouteJsConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(false);
			var routeFilter = new MvcRouteFilter(config.Object, _routes);

			var areaResult = routeFilter.AllowRoute(_routes["UndecoratedArea_default"]);
			Assert.IsFalse(areaResult);
		}
	}

	[TestFixture]
	public class ExposedArea : AreaTestBase
	{
		[Test]
		public void RouteInAreaCanBeExposed()
		{
			var config = new Mock<IRouteJsConfiguration>();
			var routeFilter = new MvcRouteFilter(config.Object, _routes);

			var areaResult = routeFilter.AllowRoute(_routes["ExposedArea_Hello"]);
			Assert.IsTrue(areaResult, "Controller-specific route");

			var defaultResult = routeFilter.AllowRoute(_routes["ExposedArea_default"]);
			Assert.IsTrue(defaultResult, "Default route");
		}

		[Test]
		public void ExposedAreaIsExposedWhenRoutesAreHiddenByDefault()
		{
			var config = new Mock<IRouteJsConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(false);
			var routeFilter = new MvcRouteFilter(config.Object, _routes);

			var areaResult = routeFilter.AllowRoute(_routes["ExposedArea_Hello"]);
			Assert.IsTrue(areaResult, "Controller-specific route");

			var defaultResult = routeFilter.AllowRoute(_routes["ExposedArea_default"]);
			Assert.IsTrue(defaultResult, "Default route");
		}
	}


	[TestFixture]
	public class HiddenArea : AreaTestBase
	{
		[Test]
		public void RouteInAreaCanBeHidden()
		{
			var config = new Mock<IRouteJsConfiguration>();
			var routeFilter = new MvcRouteFilter(config.Object, _routes);

			var areaResult = routeFilter.AllowRoute(_routes["HiddenArea_Hello"]);
			Assert.IsFalse(areaResult, "Controller-specific route");

			var defaultResult = routeFilter.AllowRoute(_routes["HiddenArea_Hello"]);
			Assert.IsFalse(defaultResult, "Default route");
		}
	}
}
