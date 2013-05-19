using System.Collections.Generic;
using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Main class for RouteJS. Handles retrieving the list of routes.
	/// </summary>
	public class RouteJs
	{
		private readonly RouteCollection _routeCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="RouteJs" /> class.
		/// </summary>
		/// <param name="routeCollection">The route collection.</param>
		public RouteJs(RouteCollection routeCollection)
		{
			_routeCollection = routeCollection;
		}

		/// <summary>
		/// Gets all the JavaScript-visible routes.
		/// </summary>
		/// <returns>A list of JavaScript-visible routes</returns>
		public IEnumerable<RouteInfo> GetRoutes()
		{
			var routes = _routeCollection.GetRoutes();

			foreach (var routeBase in routes)
			{
				if (routeBase is Route)
				{
					yield return GetRoute((Route)routeBase);
				}
			}
		}

		/// <summary>
		/// Gets information about the specified route
		/// </summary>
		/// <param name="route">The route</param>
		/// <returns>Route information</returns>
		private RouteInfo GetRoute(Route route)
		{			
			var routeInfo = new RouteInfo
			{
				Url = route.Url,
				Defaults = route.Defaults ?? new RouteValueDictionary(),
				Constraints = route.Constraints ?? new RouteValueDictionary()
			};

			return routeInfo;
		}
	}
}
