using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RouteJs.AspNet
{
	/// <summary>
	/// ASP.NET MVC controller for RouteJs. Renders JavaScript to handle routing of ASP.NET URLs.
	/// </summary>
	public class RouteJsController : Controller
    {
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
			// TODO: Load script from resource
			// TODO: dev vs prod script
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

			// TODO: This needs to be wrapped in the routing script
			return Content(jsonRoutes);
		}
    }
}
