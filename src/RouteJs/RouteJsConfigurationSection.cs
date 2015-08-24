using System.Configuration;

namespace RouteJs
{
	/// <summary>
	/// Implementation of <see cref="IRouteJsConfiguration"/> using an ASP.NET configuration section.
	/// </summary>
	public class RouteJsConfigurationSection : ConfigurationSection, IRouteJsConfiguration
	{
		/// <summary>
		/// Gets whether to expose all routes to the site.
		/// If <c>true</c>, all routes will be exposed unless explicitly hidden using <see cref="HideRoutesInJavaScriptAttribute" />.
		/// If <c>false</c>, all routes will be hidden unless explicitly exposed using <see cref="ExposeRoutesInJavaScriptAttribute" />.
		/// </summary>
		[ConfigurationProperty("exposeAllRoutes", DefaultValue = true)]
		public bool ExposeAllRoutes
		{
			get { return (bool) this["exposeAllRoutes"]; }
			set { this["exposeAllRoutes"] = value; }
		}
	}
}
