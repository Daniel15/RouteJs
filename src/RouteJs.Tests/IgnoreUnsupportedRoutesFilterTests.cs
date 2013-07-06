using System.Web;
using System.Web.Routing;
using NUnit.Framework;

namespace RouteJs.Tests
{
	[TestFixture]
	public class IgnoreUnsupportedRoutesFilterTests
	{
		[Test]
		public void AllowCustomRouteClasses()
		{
			var route = new MostAwesomeRouteEver();
			var filter = new IgnoreUnsupportedRoutesFilter();
			Assert.IsTrue(filter.AllowRoute(route));
		}

		[Test]
		public void IgnoreWebFormsRoutes()
		{
			var routeCollection = new RouteCollection();
			routeCollection.MapPageRoute("Hello", "Foo", "~/Bar.aspx");
			var filter = new IgnoreUnsupportedRoutesFilter();
			Assert.IsFalse(filter.AllowRoute(routeCollection[0]));
		}


		[Test]
		public void IgnoreWebFormsRoutes2()
		{
			var route = new Route("Foo", new PageRouteHandler("~/Bar.aspx"));
			var filter = new IgnoreUnsupportedRoutesFilter();
			Assert.IsFalse(filter.AllowRoute(route));
		}

		[Test]
		public void IgnoreIgnoreRoutes()
		{
			var routeCollection = new RouteCollection();
			routeCollection.Ignore("IgnoreMe");
			var filter = new IgnoreUnsupportedRoutesFilter();
			Assert.IsFalse(filter.AllowRoute(routeCollection[0]));
		}

		private class MostAwesomeRouteEver : RouteBase
		{
			public override RouteData GetRouteData(HttpContextBase httpContext)
			{
				throw new System.NotImplementedException();
			}

			public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
			{
				throw new System.NotImplementedException();
			}
		}
	}
}
