using System.Web.Routing;
using Moq;
using RouteJs.Mvc;

namespace RouteJs.Tests.Mvc
{
	/// <summary>
	/// Base class for all ASP.NET MVC-based RouteJs tests
	/// </summary>
	public abstract class MvcTestBase
	{
		/// <summary>
		/// Creates a new <see cref="RouteJs"/> based on the specified route collection. Uses the
		/// default configuration for ASP.NET MVC routes
		/// </summary>
		/// <param name="routes">Routes</param>
		/// <returns>RouteJs object</returns>
		protected static RouteJs CreateRouteJs(RouteCollection routes)
		{
			var config = new Mock<IRouteJsConfiguration>();
			config.Setup(x => x.ExposeAllRoutes).Returns(true);

			return new RouteJs(routes,
				routeFilters: new[] { new MvcRouteFilter(config.Object, routes) }, 
				defaultsProcessors: new[] { new DefaultsProcessor() },
				constraintsProcessors: new IConstraintsProcessor[] { });
		}
	}
}
