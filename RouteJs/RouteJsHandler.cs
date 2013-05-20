using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
		private static readonly TimeSpan _cacheFor = new TimeSpan(24, 0, 0);

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
			var javascript = GetJavaScript(RouteTable.Routes);

			if (!string.IsNullOrWhiteSpace(context.Request.Url.Query.TrimStart('?')))
			{
				SendCachingHeaders(context, javascript);
			}

			context.Response.ContentType = "text/javascript";
			context.Response.Write(javascript);
		}

		/// <summary>
		/// Send headers to cache the RouteJs response
		/// </summary>
		/// <param name="context">HTTP context</param>
		/// <param name="output">Output of the handler</param>
		private void SendCachingHeaders(HttpContext context, string output)
		{
			context.Response.Cache.SetETag(Hash(output));
			context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
			context.Response.Cache.SetExpires(DateTime.Now + _cacheFor);
			context.Response.Cache.SetMaxAge(_cacheFor);
			context.Response.Cache.SetVaryByCustom("*");
		}

		/// <summary>
		/// Gets the JavaScript routes
		/// </summary>
		/// <param name="routeCollection">The routes to render</param>
		/// <returns>JavaScript for the routes</returns>
		private static string GetJavaScript(RouteCollection routeCollection)
		{
			var jsonRoutes = GetJsonData(routeCollection);
			string content;

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RouteJs.router.js"))
			using (var reader = new StreamReader(stream))
			{
				content = reader.ReadToEnd();
			}

			return content.Replace("//{ROUTES}", "window.Router = new RouteManager(" + jsonRoutes + ");");
		}

		/// <summary>
		/// Gets the JSON data representing the routes
		/// </summary>
		/// <param name="routeCollection">The routes to render</param>
		/// <returns>JavaScript for the routes</returns>
		private static string GetJsonData(RouteCollection routeCollection)
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
			var javascript = GetJavaScript(RouteTable.Routes);
			return VirtualPathUtility.ToAbsolute("~/routejs.axd") + "?" + Hash(javascript);
		}
		#endregion
	}
}
