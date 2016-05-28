using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace RouteJs
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

		/// <summary>
		/// Gets the order of this route fetch relative to others. Fetches with smaller numbers
		/// will have their routes listed earlier in the overall route list.
		/// </summary>
		int Order { get; }
	}
}
