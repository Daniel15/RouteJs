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

		// This matches all words in a url except for those inside  parens ("{}") because those
		// are route values and they shouldn't be converted to lower case.
		private static readonly string s_lowerCasePatternMatcher = @"(\w+\/\w*)";

		/// <summary>
		/// Initializes a new instance of the <see cref="RouteJs" /> class.
		/// </summary>
		/// <param name="routeCollection">The route collection.</param>
		/// <param name="routeFilters">Any filters to apply to the routes</param>
		/// <param name="defaultsProcessors">Handler to handle processing of default values</param>
		/// <param name="constraintsProcessors">Handler to handle processing of constraints</param>
		public RouteJs(
			RouteCollection routeCollection, 
			IEnumerable<IRouteFilter> routeFilters,
			IEnumerable<IDefaultsProcessor> defaultsProcessors,
			IEnumerable<IConstraintsProcessor> constraintsProcessors
		)
		{
			_routeCollection = routeCollection;
			_routeFilters = routeFilters;
			_defaultsProcessors = defaultsProcessors;
			_constraintsProcessors = constraintsProcessors;
		}

		/// <summary>
		/// Gets or sets whether urls are converted to lower case.
		/// </summary>
		public static bool LowerCaseUrls { get; set; }

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

		// Example: "Posts/{postKey}/Edit" is converted to "posts/{postKey}/edit"
		// Words inside parens are not matched.
		private string GetUrl(string url)
		{
			if (!LowerCaseUrls)
			{
				return url;
			}

			return Regex.Replace(url, s_lowerCasePatternMatcher, (m) =>
			{
				return m.Value.ToLower();
			});
		}
	}
}
