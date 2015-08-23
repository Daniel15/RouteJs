using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Routing.Template;

namespace RouteJs
{
	/// <summary>
	/// Retrieves information on all the standard template routes in the site.
	/// </summary>
    public class TemplateRouteFetcher : IRouteFetcher
    {
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
		private static RouteInfo ProcessTemplateRoute(TemplateRoute route)
		{
			var info = new RouteInfo
			{
				Defaults = route.Defaults.ToDictionary(x => x.Key, x => x.Value),
				Optional = new List<string>(),
			};
			// TODO Constraints = route.Constraints,

			var parsedTemplate = TemplateParser.Parse(route.RouteTemplate);
			var builder = new StringBuilder();
			foreach (var segment in parsedTemplate.Segments)
			{
				foreach (var part in segment.Parts)
				{
					if (part.IsLiteral)
					{
						builder.Append(part.Text);
						continue;
					}
					if (part.DefaultValue != null)
					{
						info.Defaults[part.Name] = part.DefaultValue;
					}
					if (part.IsOptional)
					{
						info.Optional.Add(part.Name);
					}
					builder.Append('{');
					builder.Append(part.Name);
					builder.Append('}');
					// TODO part.InlineConstraints
					// TODO part.IsOptionalSeperator
					// TODO part.IsCatchAll
				}
				builder.Append('/');
			}
			info.Url = builder.ToString().TrimEnd('/');

			return info;
		}
	}
}
