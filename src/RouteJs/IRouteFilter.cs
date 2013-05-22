using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Provides the ability to hide certain routes from the JavaScript output. Multiple IRouteFilters
	/// can be registered, in which case they are all used.
	/// </summary>
	public interface IRouteFilter
	{
		/// <summary>
		/// Check whether the specified route should be exposed in the JavaScript output
		/// </summary>
		/// <param name="routeBase">Route to check</param>
		/// <returns><c>false</c> if the route should definitely be blocked, <c>true</c> if the route should be exposed (or unsure)</returns>
		bool AllowRoute(RouteBase routeBase);
	}
}
