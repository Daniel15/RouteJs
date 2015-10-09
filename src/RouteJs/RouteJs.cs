using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Main class for RouteJS. Handles retrieving the list of routes.
	/// </summary>
	public class RouteJs
	{
		private readonly RouteCollection _routeCollection;
		private readonly IEnumerable<IRouteFilter> _routeFilters;
		private readonly IEnumerable<IDefaultsProcessor> _defaultsProcessors;
		private readonly IEnumerable<IConstraintsProcessor> _constraintsProcessors;
		private readonly IRouteJsConfiguration _routeJsConfiguration;

		// This matches all words in a url except for those inside parens ("{}") because those
		// are route values and they shouldn't be converted to lowercase.
		private static readonly string _lowerCasePatternMatcher = @"(\w+\/\w*)";
		private static readonly string[] _defaultKeysToConvert = new[] { "controller", "action" };

		/// <summary>
		/// Initializes a new instance of the <see cref="RouteJs" /> class.
		/// </summary>
		/// <param name="routeCollection">The route collection.</param>
		/// <param name="routeFilters">Any filters to apply to the routes</param>
		/// <param name="defaultsProcessors">Handler to handle processing of default values</param>
		/// <param name="constraintsProcessors">Handler to handle processing of constraints</param>
		/// <param name="routeJsConfiguration">Configuration object that contains options</param>
		public RouteJs(
			RouteCollection routeCollection,
			IEnumerable<IRouteFilter> routeFilters,
			IEnumerable<IDefaultsProcessor> defaultsProcessors,
			IEnumerable<IConstraintsProcessor> constraintsProcessors,
			IRouteJsConfiguration routeJsConfiguration
		)
		{
			_routeCollection = routeCollection;
			_routeFilters = routeFilters;
			_defaultsProcessors = defaultsProcessors;
			_constraintsProcessors = constraintsProcessors;
			_routeJsConfiguration = routeJsConfiguration;
		}

		/// <summary>
		/// Gets all the JavaScript-visible routes.
		/// </summary>
		/// <returns>A list of JavaScript-visible routes</returns>
		public IEnumerable<RouteInfo> GetRoutes()
		{
			var routes = _routeCollection.GetRoutes();

			foreach (var routeBase in routes.Where(AllowRoute))
			{
				if (routeBase is Route)
				{
					yield return GetRoute((Route)routeBase);
				}
			}
		}

		/// <summary>
		/// Check whether this route should be exposed in the JavaScript
		/// </summary>
		/// <param name="route">Route to check</param>
		/// <returns><c>true</c> if the route should be exposed</returns>
		private bool AllowRoute(RouteBase route)
		{
			return _routeFilters.All(filter => filter.AllowRoute(route));
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
				Url = GetUrl(route.Url),
				Constraints = new RouteValueDictionary(),
			};

			if (_routeJsConfiguration.LowerCaseUrls)
			{
				// Convert "controller" and "action" defaults to lowercase.
				var convertedDefaults = route.Defaults
					.Where(d => _defaultKeysToConvert.Any(st => string.Equals(d.Key, st, StringComparison.OrdinalIgnoreCase)))
					.Select(d => new KeyValuePair<string, object>(d.Key, ((string)d.Value).ToLower()))
					.ToArray();
				foreach (var item in convertedDefaults)
				{
					route.Defaults[item.Key] = item.Value;
				}
			}

			foreach (var processor in _defaultsProcessors)
			{
				processor.ProcessDefaults(route, routeInfo);
			}
			if (route.Constraints != null)
			{
				foreach (var processor in _constraintsProcessors)
				{
					processor.ProcessConstraints(route.Constraints, routeInfo);
				}	
			}

			return routeInfo;
		}

		/// <summary>
		/// Converts to lowercase url only if LowerCaseUrls options is specified.
		/// Example: "Posts/{postKey}/Edit" is converted to "posts/{postKey}/edit"
		/// Words inside parens are not matched.
		/// </summary>
		/// <param name="url">The url to convert.</param>
		/// <returns>The converted url if LowerCaseUrls is specified, or the same url otherwise.</returns>
		private string GetUrl(string url)
		{
			if (!_routeJsConfiguration.LowerCaseUrls)
			{
				return url;
			}

			return Regex.Replace(url, _lowerCasePatternMatcher, (m) =>
			{
				return m.Value.ToLower();
			});
		}
	}
}
