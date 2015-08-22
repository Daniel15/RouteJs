using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Microsoft.Net.Http.Headers;

namespace RouteJs.AspNet
{
	/// <summary>
	/// ASP.NET MVC controller for RouteJs. Renders JavaScript to handle routing of ASP.NET URLs.
	/// </summary>
	public class RouteJsController : Controller
	{
		/// <summary>
		/// Reference to the RouteJs assembly
		/// </summary>
		private static readonly Assembly _assembly = typeof (RouteJsController).GetTypeInfo().Assembly;
		/// <summary>
		/// Route fetchers used to get information about the routes.
		/// </summary>
		private readonly IEnumerable<IRouteFetcher> _routeFetchers;

		public RouteJsController(IEnumerable<IRouteFetcher> routeFetchers)
		{
			_routeFetchers = routeFetchers;
		}

		/// <summary>
		/// Renders the RouteJs script file, and the routes for the current site.
		/// </summary>
		/// <returns></returns>
		[Route("/_routejs")]
		public IActionResult Routes()
		{
			// TODO: Hash for long-term caching
			// TODO: dev vs prod script

			var javascript = GetJavaScript();
			var response = Content(javascript);
			response.ContentType = MediaTypeHeaderValue.Parse("application/javascript");
			return response;
		}

		/// <summary>
		/// Gets the JavaScript routes
		/// </summary>
		/// <returns>JavaScript for the routes</returns>
		private string GetJavaScript()
		{
			var jsonRoutes = GetJsonData();
			var content = _assembly.GetResourceScript("RouteJs.AspNet.compiler.resources.router.js");
			return content + "window.Router = new RouteJs.RouteManager(" + jsonRoutes + ");";
		}

		/// <summary>
		/// Gets the JSON data representing the routes
		/// </summary>
		/// <returns>JavaScript for the routes</returns>
		private string GetJsonData()
		{
			var routes = _routeFetchers.SelectMany(x => x.GetRoutes(RouteData));
			var settings = new
			{
				Routes = routes,
				BaseUrl = Url.Content("~"),
			};
			var jsonRoutes = JsonConvert.SerializeObject(settings, new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			});
			return jsonRoutes;
		}
	}
}
