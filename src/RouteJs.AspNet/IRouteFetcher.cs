using System.Collections.Generic;
using Microsoft.AspNet.Routing;

namespace RouteJs.AspNet
{
	/// <summary>
	/// Handles retrieving information about a particular type of route.
	/// </summary>
	public interface IRouteFetcher
	{
		/// <summary>
		/// Gets all route information supported by this fetcher.
		/// </summary>
		/// <param name="routeData">Raw route data from ASP.NET MVC</param>
		/// <returns>Processed route information</returns>
		IEnumerable<RouteInfo> GetRoutes(RouteData routeData);
	}
}
