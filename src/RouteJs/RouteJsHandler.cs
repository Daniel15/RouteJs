using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using RouteJs.TinyIoc;

namespace RouteJs
{
	/// <summary>
	/// ASP.NET handler for RouteJS. Renders JavaScript to handle routing of ASP.NET URLs
	/// </summary>
	public class RouteJsHandler : IHttpHandler
	{
		/// <summary>
		/// Lazy initialiser for IoC container
		/// </summary>
		private static readonly Lazy<TinyIoCContainer> _container = new Lazy<TinyIoCContainer>(InitialiseIoC);
		/// <summary>
		/// How long to cache the JavaScript output for. Only used when a unique hash is present in the URL.
		/// </summary>
		private static readonly TimeSpan _cacheFor = new TimeSpan(365, 0, 0, 0);

		/// <summary>
		/// IoC container
		/// </summary>
		private static TinyIoCContainer Container { get { return _container.Value; } }

		/// <summary>
		/// Initialises the IoC container.
		/// </summary>
		/// <returns>The IoC container</returns>
		private static TinyIoCContainer InitialiseIoC()
		{
			var container = TinyIoCContainer.Current;
			ComponentRegistration.RegisterAll(container);
			return container;
		}

		/// <summary>
		/// Handle a HTTP request
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
		public void ProcessRequest(HttpContext context)
		{
			ProcessRequest(new HttpContextWrapper(context));
		}

		/// <summary>
		/// Handle a HTTP request
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
		public void ProcessRequest(HttpContextBase context)
		{
			bool sendCacheHeaders;
			bool sendFileNotFound;
			var debugMode = CheckDebugMode(context.Request, out sendCacheHeaders, out sendFileNotFound);

			if (sendFileNotFound)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				context.Response.End();
				return;
			}

			var javascript = GetJavaScript(debugMode);

			if (sendCacheHeaders)
			{
				SendCachingHeaders(context, javascript);
			}

			context.Response.ContentType = "text/javascript";
			context.Response.Write(javascript);
		}

		/// <summary>
		/// Checks whether the specified request is a debug mode request (unminified JavaScript) or 
		/// release mode (minified JavaScript)
		/// </summary>
		/// <param name="request">HTTP request</param>
		/// <param name="sendCacheHeaders">Whether to send caching headers</param>
		/// <param name="sendFileNotFound">Whether to send a file not found error</param>
		/// <returns><c>true</c> if the specified request is a debug mode request</returns>
		public static bool CheckDebugMode(HttpRequestBase request, out bool sendCacheHeaders, out bool sendFileNotFound)
		{
			sendFileNotFound = false;
			sendCacheHeaders = false;

			// Check if any path info was provided
			var pathInfo = request.PathInfo.Split('/');
			if (pathInfo.Length < 2)
			{
				// Not enough path info for a full path - User could be hitting routejs.axd directly with no params
				// In this case, just serve debug version of the JavaScript
				return true;
			}

			switch (pathInfo.Last())
			{
				case "router.min.js":
					sendCacheHeaders = true;
					return false;

				case "router.js":
					sendCacheHeaders = true;
					return true;

				default:
					// Send a 404, invalid file name
					sendFileNotFound = true;
					return true;
			}
		}

		/// <summary>
		/// Send headers to cache the RouteJs response
		/// </summary>
		/// <param name="context">HTTP context</param>
		/// <param name="output">Output of the handler</param>
		private static void SendCachingHeaders(HttpContextBase context, string output)
		{
			context.Response.Cache.SetETag(Hash(output));
			// Cache on both the server- and client-side
			context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
			context.Response.Cache.SetExpires(DateTime.Now + _cacheFor);
			context.Response.Cache.SetMaxAge(_cacheFor);
			context.Response.Cache.SetVaryByCustom("*");
		}

		/// <summary>
		/// Gets the JavaScript routes
		/// </summary>
		/// <returns>JavaScript for the routes</returns>
		private static string GetJavaScript(bool debugMode)
		{
			var resourceName = debugMode ? "RouteJs.router.js" : "RouteJs.router.min.js";
			var jsonRoutes = GetJsonData();
			string content;

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			using (var reader = new StreamReader(stream))
			{
				content = reader.ReadToEnd();
			}

			// Replace version number in content
			content = content.Replace("{VERSION}", GetVersionNumber());

			return content + "window.Router = new RouteJs.RouteManager(" + jsonRoutes + ");";
		}

		/// <summary>
		/// Gets the RouteJs version number
		/// </summary>
		/// <returns>The RouteJs version number</returns>
		public static string GetVersionNumber()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var rawVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
			var lastDot = rawVersion.LastIndexOf('.');
			var version = rawVersion.Substring(0, lastDot);
			var build = rawVersion.Substring(lastDot + 1);
			return string.Format("{0} (build {1})", version, build);
		}

		/// <summary>
		/// Gets the JSON data representing the routes
		/// </summary>
		/// <returns>JavaScript for the routes</returns>
		private static string GetJsonData()
		{
			var router = Container.Resolve<RouteJs>();
			var routes = router.GetRoutes();
			var settings = new
			{
				Routes = routes,
				BaseUrl = VirtualPathUtility.ToAbsolute("~/")
			};
			var jsonRoutes = JsonConvert.SerializeObject(settings, new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			});

			return jsonRoutes;
		}

		public bool IsReusable
		{
			get { return true; }
		}

		/// <summary>
		/// Calculate the SHA1 hash of the specified content
		/// </summary>
		/// <param name="content">Content to hash</param>
		/// <returns>Hash</returns>
		private static string Hash(string content)
		{
			using (var sha1 = SHA1.Create())
			{
				var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(content));
				var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
				return hash;
			}
		}

		#region Handler URL retrieval
		private static readonly Lazy<string> _handlerUrl = new Lazy<string>(GetHandlerUrl);

		/// <summary>
		/// Gets the URL to the RouteJs handler, with a unique hash in the URL. The hash will change 
		/// every time a route changes
		/// </summary>
		public static string HandlerUrl
		{         
			get { return _handlerUrl.Value; }
		}

		/// <summary>
		/// Gets the URL to the RouteJs handler, with a unique hash in the URL. The hash will change 
		/// every time a route changes
		/// </summary>
		/// <returns>URL to the handler</returns>
		private static string GetHandlerUrl()
		{
			var debugMode = HttpContext.Current.IsDebuggingEnabled;
			// Hash the file contents so the URL can change whenever a route or the routing JS changes
			var javascript = GetJavaScript(debugMode);
			var hash = Hash(javascript);

			// Serve minified JavaScript when running in release mode
			var filename = debugMode ? "router.js" : "router.min.js";

			return string.Join("/", VirtualPathUtility.ToAbsolute("~/routejs.axd"), hash, filename);
		}
		#endregion
	}
}
