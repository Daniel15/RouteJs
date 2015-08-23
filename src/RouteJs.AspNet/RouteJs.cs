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
		/// Context object for the current request.
		/// </summary>
		private readonly ActionContext _actionContext;

		public RouteJs(IEnumerable<IRouteFetcher> routeFetchers, IScopedInstance<ActionContext> actionContextAccessor)
		{
			_routeFetchers = routeFetchers;
			_actionContext = actionContextAccessor.Value;
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
			var routes = _routeFetchers.SelectMany(x => x.GetRoutes(_actionContext.RouteData));
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
