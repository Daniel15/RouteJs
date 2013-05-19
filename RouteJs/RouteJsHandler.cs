using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
		/// Handle a HTTP request
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
		public void ProcessRequest(HttpContext context)
		{
			var router = new RouteJs(RouteTable.Routes);
			var routes = router.GetRoutes();
			var js = GetJavaScript(routes);

			context.Response.ContentType = "application/json";
			context.Response.Write(js);
		}

		/// <summary>
		/// Gets the JavaScript routes
		/// </summary>
		/// <param name="routes">The routes to render</param>
		/// <returns>JavaScript for the routes</returns>
		private string GetJavaScript(IEnumerable<RouteInfo> routes)
		{
			var jsonRoutes = JsonConvert.SerializeObject(routes, new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			});
			string content;

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RouteJs.router.js"))
			using (var reader = new StreamReader(stream))
			{
				content = reader.ReadToEnd();
			}

			return content.Replace("//{ROUTES}", "window.Router = new RouteManager(" + jsonRoutes + ");");
		}

		public bool IsReusable
		{
			get { return true; }
		}
	}
}
