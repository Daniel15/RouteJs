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
		/// Name of the data token used to store area
		/// </summary>
		private const string AREA_TOKEN = "area";

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

			// Add area if it's specified in the route
			if (route.DataTokens.ContainsKey(AREA_TOKEN))
			{
				routeInfo.Defaults.Add("area", route.DataTokens[AREA_TOKEN]);
			}
		}
	}
}
