using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Exclude unsupported routes from the output
	/// </summary>
	public class IgnoreUnsupportedRoutesFilter : IRouteFilter
	{
		/// <summary>
		/// Check whether the specified route should be exposed in the JavaScript output
		/// </summary>
		/// <param name="routeBase">Route to check</param>
		/// <returns><c>false</c> if the route should definitely be blocked, <c>true</c> if the route should be exposed (or unsure)</returns>
		public bool AllowRoute(RouteBase routeBase)
		{
			var route = routeBase as Route;
			if (route == null)
				return true;

			// WebForms - Not supported
			if (route.RouteHandler is PageRouteHandler)
				return false;

			// Ignore routes
			if (route.RouteHandler is StopRoutingHandler)
				return false;

			// ASP.NET WebAPI (https://github.com/Daniel15/RouteJs/issues/9)
			// Ugly hack so we don't have to reference the WebAPI assembly for now.
			if (route.GetType().FullName == "System.Web.Http.WebHost.Routing.HttpWebRoute")
				return false;

			return true;
		}
	}
}
