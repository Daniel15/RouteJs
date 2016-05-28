using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace RouteJs
{
	/// <summary>
	/// ASP.NET MVC controller for RouteJs. Renders JavaScript to handle routing of ASP.NET URLs.
	/// </summary>
	public class RouteJsController : Controller
	{
		/// <summary>
		/// How long to cache the JavaScript output for. Only used when a unique hash is present in the URL.
		/// </summary>
		private static readonly TimeSpan _cacheFor = new TimeSpan(365, 0, 0, 0);

		/// <summary>
		/// Core RouteJs class - Actually does all the work.
		/// </summary>
		private readonly IRouteJs _routeJs;

		/// <summary>
		/// Creates a new instance of the main RouteJs controller.
		/// </summary>
		/// <param name="routeJs"></param>
		public RouteJsController(IRouteJs routeJs)
		{
			_routeJs = routeJs;
		}

		/// <summary>
		/// Renders the RouteJs script file, and the routes for the current site.
		/// </summary>
		/// <returns></returns>
		[Route("/_routejs/{hash?}/{environment:regex(" + RouteJsHelper.MINIFIED_MODE + ")?}")]
		public IActionResult Routes(string hash, string environment)
		{
			var debugMode = environment != RouteJsHelper.MINIFIED_MODE;
			var shouldCache = hash != null;

			var javascript = _routeJs.GetJavaScript(debugMode);
			if (shouldCache)
			{
				Response.Headers.Add("Expires", (DateTime.Now + _cacheFor).ToString("R"));
				Response.Headers.Add("Cache-control", "public, max-age=" + _cacheFor.TotalSeconds.ToString(CultureInfo.InvariantCulture));
			}
			return Content(javascript, "text/javascript");
		}
	}
}
