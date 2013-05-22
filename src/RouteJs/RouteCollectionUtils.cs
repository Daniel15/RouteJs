using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Utilities for ASP.NET URL routing.
	/// </summary>
	/// <remarks>
	/// Unfortunately, the list of routes is private, so we need to use reflection to get to it :(
	/// </remarks>
	public static class RouteCollectionUtils
	{
		/// <summary>
		/// Method used to get routes from a route collection.
		/// </summary>
		private static readonly PropertyInfo _getRoutes;

		static RouteCollectionUtils()
		{
			_getRoutes = typeof(Collection<RouteBase>).GetProperty("Items", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		/// <summary>
		/// Get the routes in the specified route collection.
		/// </summary>
		/// <param name="routeCollection"></param>
		/// <returns></returns>
		public static IList<RouteBase> GetRoutes(this RouteCollection routeCollection)
		{
			return (IList<RouteBase>)_getRoutes.GetValue(routeCollection, null);
		}
	}
}

