using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace RouteJs.Mvc4
{
	/// <summary>
	/// Handles parsing of optional parameters from the defaults in MVC routes
	/// </summary>
	public class DefaultsProcessor : IDefaultsProcessor
	{
		/// <summary>
		/// Process the defaults of the specified route
		/// </summary>
		/// <param name="route">Route to process</param>
		/// <param name="routeInfo">Output information about the route</param>
		public void ProcessDefaults(Route route, RouteInfo routeInfo)
		{
			routeInfo.Defaults = new RouteValueDictionary();
			routeInfo.Optional = new List<string>();

			if (route.Defaults == null)
				return;

			foreach (var kvp in route.Defaults)
			{
				if (kvp.Value == UrlParameter.Optional)
				{
					routeInfo.Optional.Add(kvp.Key);
				}
				else
				{
					routeInfo.Defaults.Add(kvp.Key, kvp.Value);
				}
			}
		}
	}
}
