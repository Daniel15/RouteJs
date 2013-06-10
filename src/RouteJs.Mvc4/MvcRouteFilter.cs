using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

namespace RouteJs.Mvc
{
	/// <summary>
	/// Filters the rendered routes based on the <see cref="ExposeRoutesInJavaScriptAttribute"/> and 
	/// <see cref="HideRoutesInJavaScriptAttribute"/> attributes on ASP.NET MVC controllers.
	/// </summary>
	public class MvcRouteFilter : IRouteFilter
	{
		/// <summary>
		/// RouteJs configuration
		/// </summary>
		private readonly IConfiguration _configuration;
		/// <summary>
		/// Whitelist of controllers whose routes are always rendered
		/// </summary>
		private HashSet<string> _controllerWhitelist;
		/// <summary>
		/// Blacklist of controllers whose routes are never rendered
		/// </summary>
		private HashSet<string> _controllerBlacklist;

		/// <summary>
		/// Initializes a new instance of the <see cref="MvcRouteFilter" /> class.
		/// </summary>
		/// <param name="configuration">The RouteJs configuration.</param>
		public MvcRouteFilter(IConfiguration configuration)
		{
			_configuration = configuration;
			BuildLists();
		}

		/// <summary>
		/// Build the controller whitelist and blacklist
		/// </summary>
		private void BuildLists()
		{
			// Get all the controllers from all loaded assemblies
			var controllers = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => type.IsSubclassOf(typeof(Controller)))
				.ToList();

			// Check which ones implement the attributes
			_controllerWhitelist = ControllersImplementingAttribute(controllers, typeof(ExposeRoutesInJavaScriptAttribute));
			_controllerBlacklist = ControllersImplementingAttribute(controllers, typeof(HideRoutesInJavaScriptAttribute));
		}


		/// <summary>
		/// Returns all the controllers that implement the specified attribute
		/// </summary>
		/// <param name="types">The controller types.</param>
		/// <param name="attributeType">Type of the attribute to check for.</param>
		/// <returns>All the types that implement the specified attribute</returns>
		private HashSet<string> ControllersImplementingAttribute(IEnumerable<Type> types, Type attributeType)
		{
			return new HashSet<string>(types
				.Where(c => c.IsDefined(attributeType, true))
				// Remove "Controller" from the class name as it's not used when referencing the controller
				.Select(c => c.Name.Replace("Controller", string.Empty))
			);
		}

		/// <summary>
		/// Check whether the specified route should be exposed in the JavaScript output
		/// </summary>
		/// <param name="routeBase">Route to check</param>
		/// <returns>
		///   <c>false</c> if the route should definitely be blocked, <c>true</c> if the route should be exposed (or unsure)
		/// </returns>
		public bool AllowRoute(RouteBase routeBase)
		{
			// Allow if we don't know what it is (another filter can take care of it)
			var route = routeBase as Route;
			if (route == null)
				return true;

			// Allow if there's no controller specified
			if (route.Defaults == null || !route.Defaults.ContainsKey("controller"))
				return true;

			var controller = route.Defaults["controller"].ToString();

			// If explicitly blacklisted, always deny
			if (_controllerBlacklist.Contains(controller))
				return false;

			// If explicitly whitelisted, always allow
			if (_controllerWhitelist.Contains(controller))
				return true;

			// Otherwise, allow based on configuration
			return _configuration.ExposeAllRoutes;
		}
	}
}
