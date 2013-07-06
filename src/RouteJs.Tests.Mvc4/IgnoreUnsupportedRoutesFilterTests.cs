using System.Web.Http;
using System.Web.Routing;
using NUnit.Framework;
using System.Web.Mvc;

namespace RouteJs.Tests.Mvc
{
	[TestFixture]
	public class IgnoreUnsupportedRoutesFilterTests
	{
		[Test]
		public void IgnoreWebApiRoute()
		{
			var routeCollection = new RouteCollection();
			routeCollection.MapHttpRoute(
				name: "Hello",
				routeTemplate: "hello/world/{id}"
			);

			var filter = new IgnoreUnsupportedRoutesFilter();
			Assert.IsFalse(filter.AllowRoute(routeCollection[0]));
		}

		[Test]
		public void AllowMvcRoute()
		{
			var routeCollection = new RouteCollection();
			routeCollection.MapRoute(
				name: "hello",
				url: "hello/world",
				defaults: new {controller = "HelloWorld", action = "Hello"}
			);

			var filter = new IgnoreUnsupportedRoutesFilter();
			Assert.IsTrue(filter.AllowRoute(routeCollection[0]));
		}
	}
}
