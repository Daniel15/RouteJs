using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Routing.Template;

namespace RouteJs
{
	/// <summary>
	/// Retrieves information on all the standard template routes in the site.
	/// </summary>
    public class TemplateRouteFetcher : IRouteFetcher
    {
		private readonly IConstraintsProcessor _constraintsProcessor;
		private readonly IRouteTemplateParser _parser;

		public TemplateRouteFetcher(IConstraintsProcessor constraintsProcessor, IRouteTemplateParser parser)
		{
			_constraintsProcessor = constraintsProcessor;
			_parser = parser;
		}

		/// <summary>
		/// Gets the order of this route fetch relative to others. Fetches with smaller numbers
		/// will have their routes listed earlier in the overall route list.
		/// </summary>
		public int Order => 99;

		/// <summary>
		/// Gets the route information
		/// </summary>
		/// <param name="routeData">Raw route data from ASP.NET MVC</param>
		/// <returns>Processed route information</returns>
		public IEnumerable<RouteInfo> GetRoutes(RouteData routeData)
		{
			var routeCollection = routeData.Routers.OfType<RouteCollection>().First();
			for (var i = 0; i < routeCollection.Count; i++)
			{
				var route = routeCollection[i];
				if (route is TemplateRoute)
				{
					yield return ProcessTemplateRoute((TemplateRoute)route);
				}
			}
		}

		/// <summary>
		/// Gets useful information from the specified template route
		/// </summary>
		/// <param name="route">Template route to get information from</param>
		/// <returns>Information from the route</returns>
		private RouteInfo ProcessTemplateRoute(TemplateRoute route)
		{
			var info = new RouteInfo
			{
				Constraints = _constraintsProcessor.ProcessConstraints(route.Constraints),
				Defaults = route.Defaults.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value),
				Optional = new List<string>(),
			};
			var template = TemplateParser.Parse(route.RouteTemplate);
			_parser.Parse(template, info);

			return info;
		}
	}
}
