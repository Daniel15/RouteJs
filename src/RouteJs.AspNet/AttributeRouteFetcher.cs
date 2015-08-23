using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Routing;

namespace RouteJs
{
	/// <summary>
	/// Gets information about the attribute routes in the site
	/// </summary>
    public class AttributeRouteFetcher : IRouteFetcher
    {
		/// <summary>
		/// Gets the route information
		/// </summary>
		/// <param name="routeData">Raw route data from ASP.NET MVC</param>
		/// <returns>Processed route information</returns>
		public IEnumerable<RouteInfo> GetRoutes(RouteData routeData)
	    {
			// TODO: Implement
		    return Enumerable.Empty<RouteInfo>();
	    }
    }
}
