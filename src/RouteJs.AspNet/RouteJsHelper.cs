using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Html;

namespace RouteJs
{
	/// <summary>
	/// RouteJs ASP.NET MVC helper. Handles rendering scripts tag to load the RouteJs script.
	/// </summary>
	public class RouteJsHelper : IRouteJsHelper
	{
		/// <summary>
		/// Added to the URI to specify that a minified version of the script should be served.
		/// This is done in production environments.
		/// </summary>
		public const string MINIFIED_MODE = "min";
		/// <summary>
		/// A cached version of the RouteJs script tag. Ideally this should be in a separate
		/// cache class somewhere, but this is fine for now.
		/// </summary>
		private static HtmlString _scriptTag;

		/// <summary>
		/// URL helper
		/// </summary>
	    private readonly IUrlHelperFactory _urlHelperFactory;
		/// <summary>
		/// Service provider for dependency injection
		/// </summary>
		private readonly IServiceProvider _serviceProvider;
		/// <summary>
		/// Hosting environment
		/// </summary>
		private readonly IHostingEnvironment _env;

		private readonly IActionContextAccessor _actionContextAccessor;

		/// <summary>
		/// Creates a new RouteJs helper
		/// </summary>
		/// <param name="urlHelperFactory"></param>
		/// <param name="serviceProvider">ASP.NET service provider (dependency injection)</param>
		/// <param name="env">ASP.NET environment</param>
		/// <param name="actionContextAccessor"></param>
		public RouteJsHelper(
			IUrlHelperFactory urlHelperFactory, 
			IServiceProvider serviceProvider, 
			IHostingEnvironment env,
			IActionContextAccessor actionContextAccessor)
		{
			_urlHelperFactory = urlHelperFactory;
			_serviceProvider = serviceProvider;
			_env = env;
			_actionContextAccessor = actionContextAccessor;
		}

		/// <summary>
		/// Renders the RouteJs script tag.
		/// </summary>
		/// <returns>A script tag to load RouteJs</returns>
		public HtmlString Render()
		{
			if (_scriptTag == null)
			{
				// Script tag has not been built yet, compute the hash and create it.
				var hash = ComputeHash();
				var environment = _env.IsProduction() ? MINIFIED_MODE : null;
				var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
				var url = urlHelper.Action("Routes", "RouteJs", new { hash, environment });
				_scriptTag = new HtmlString($"<script src=\"{url}\"></script>");
            }
			return _scriptTag;
	    }

		/// <summary>
		/// Computes a hash of the current RouteJs output. This is done to allow long-term
		/// caching. When any of the site's code changes, ASP.NET reloads the AppDomain, which
		/// will clear this cache.
		/// </summary>
		/// <returns>A hash of the contents</returns>
		private string ComputeHash()
		{
			// The service is loaded here rather than in the constructor so we don't incur the cost of 
			// creating it when it's not needed.
			var routeJs = _serviceProvider.GetRequiredService<IRouteJs>();
			var script = routeJs.GetJavaScript(debugMode: true);
			using (var sha1 = SHA1.Create())
			{
				var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(script));
				var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
				return hash;
			}
		}

		/// <summary>
		/// Clears the cached hash
		/// </summary>
		public static void ClearCache()
		{
			_scriptTag = null;
		}
    }
}
