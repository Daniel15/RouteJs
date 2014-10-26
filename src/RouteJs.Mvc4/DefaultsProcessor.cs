using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace RouteJs.Mvc
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

		private const string ROUTE_VALUE_DICTIONARY_KEY = "RouteValueDictionary";

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
					routeInfo.Optional.Add(kvp.Key.ToLowerInvariant());
				}
				else if (ShouldAddDefault(kvp.Key))
				{
					routeInfo.Defaults.Add(kvp.Key.ToLowerInvariant(), kvp.Value);
				}
			}

			// Add area if it's specified in the route
			if (route.DataTokens != null && route.DataTokens.ContainsKey(AREA_TOKEN))
			{
				routeInfo.Defaults.Add("area", route.DataTokens[AREA_TOKEN]);
			}
		}

		/// <summary>
		/// Determines whether the specified default value should be added to the output
		/// </summary>
		/// <param name="key">Key of the default value</param>
		/// <returns><c>true</c> if the default value should be used, or <c>false</c> if it should
		/// be ignored.</returns>
		private bool ShouldAddDefault(string key)
		{
			// T4MVC adds RouteValueDictionary to defaults, but we don't need it.
			return key != ROUTE_VALUE_DICTIONARY_KEY;
		}
	}
}
