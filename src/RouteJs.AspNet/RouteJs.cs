using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RouteJs
{
	/// <summary>
	/// Core RouteJs class. Handles fetching the available routes and generating the routing script.
	/// </summary>
	public class RouteJs : IRouteJs
	{
		/// <summary>
		/// Reference to the RouteJs assembly
		/// </summary>
		private static readonly Assembly _assembly = typeof(RouteJs).GetTypeInfo().Assembly;
		/// <summary>
		/// Route fetchers used to get information about the routes.
		/// </summary>
		private readonly IEnumerable<IRouteFetcher> _routeFetchers;
		/// <summary>
		/// Router filters used to determine which routes should not be visible in JavaScript.
		/// </summary>
		private readonly IEnumerable<IRouteFilter> _routeFilters;

		/// <summary>
		/// Context object for the current request.
		/// </summary>
		private readonly ActionContext _actionContext;

		public RouteJs(
			IEnumerable<IRouteFetcher> routeFetchers,
			IActionContextAccessor actionContextAccessor,
			IEnumerable<IRouteFilter> routeFilters
		)
		{
			_routeFetchers = routeFetchers;
			_routeFilters = routeFilters;
			_actionContext = actionContextAccessor.ActionContext;
		}

		/// <summary>
		/// Gets the JavaScript routes
		/// </summary>
		/// <returns>JavaScript for the routes</returns>
		public string GetJavaScript(bool debugMode)
		{
			var jsonRoutes = GetJsonData();
			var content = debugMode
				? _assembly.GetResourceScript("RouteJs.AspNet.compiler.resources.router.js")
				: _assembly.GetResourceScript("RouteJs.AspNet.compiler.resources.router.min.js");
			return content + "window.Router = new RouteJs.RouteManager(" + jsonRoutes + ");";
		}

		/// <summary>
		/// Gets the JSON data representing the routes
		/// </summary>
		/// <returns>JavaScript for the routes</returns>
		public string GetJsonData()
		{
			var routes = _routeFetchers
				.OrderBy(fetcher => fetcher.Order)
				.SelectMany(fetcher => fetcher.GetRoutes(_actionContext.RouteData))
				// Filter out any routes the fetcher couldn't handle correctly
				.Where(route => route != null)
				// Every filter must pass in order to use this route
				.Where(route => _routeFilters.All(filter => filter.AllowRoute(route)));

			var settings = new
			{
				Routes = routes,
				BaseUrl = _actionContext.HttpContext.Request.PathBase.Value,
			};
			var jsonRoutes = JsonConvert.SerializeObject(settings, new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			});
			return jsonRoutes;
		}
	}
}
