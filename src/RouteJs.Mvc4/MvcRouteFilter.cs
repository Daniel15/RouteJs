using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using System.Reflection;

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
		/// ASP.NET routes
		/// </summary>
		private readonly RouteCollection _routeCollection;

		/// <summary>
		/// Whitelist of controllers whose routes are always rendered
		/// </summary>
		private Dictionary<string, Type> _controllerWhitelist;
		/// <summary>
		/// Blacklist of controllers whose routes are never rendered
		/// </summary>
		private Dictionary<string, Type> _controllerBlacklist;

		/// <summary>
		/// A mapping of namespace prefix to area name
		/// </summary>
		private readonly IDictionary<string, string> _areaNamespaceMapping;
		/// <summary>
		/// Whitelist of areas whose default routes are always rendered. Areas as whitelisted if at
		/// least one controller in the area is whitelisted.
		/// </summary>
		private HashSet<string> _areaWhitelist; 

		/// <summary>
		/// Initializes a new instance of the <see cref="MvcRouteFilter" /> class.
		/// </summary>
		/// <param name="configuration">The RouteJs configuration.</param>
		/// <param name="routeCollection">The ASP.NET routes</param>
		public MvcRouteFilter(IConfiguration configuration, RouteCollection routeCollection)
		{
			_configuration = configuration;
			_routeCollection = routeCollection;

			_areaNamespaceMapping = GetAreaNamespaceMap();
			BuildLists();
		}

		/// <summary>
		/// Build the controller whitelist and blacklist
		/// </summary>
		private void BuildLists()
		{
			// Get all the controllers from all loaded assemblies
			var controllers = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(GetTypesFromAssembly)
				.Where(type => type.IsSubclassOf(typeof(Controller)))
				.ToList();

			// Check which ones implement the attributes
			_controllerWhitelist = ControllersImplementingAttribute(controllers, typeof(ExposeRoutesInJavaScriptAttribute));
			_controllerBlacklist = ControllersImplementingAttribute(controllers, typeof(HideRoutesInJavaScriptAttribute));

			// Check for exposed controllers in areas - If any controller in the area is exposed, any 
			// default routes in the area need to be exposed as well.
			_areaWhitelist = new HashSet<string>();
			foreach (var controller in _controllerWhitelist)
			{
				var areaKey = _areaNamespaceMapping.Keys.FirstOrDefault(areaNs => controller.Value.Namespace.StartsWith(areaNs));
				if (areaKey != null)
				{
					_areaWhitelist.Add(_areaNamespaceMapping[areaKey]);
				}
			}
		}
		
		/// <summary>
		/// Retrieve all the types exposed by the specified assembly
		/// </summary>
		/// <param name="assembly">Assembly to scan</param>
		/// <returns>All the types exposed by the assembly</returns>
		private IEnumerable<Type> GetTypesFromAssembly(Assembly assembly)
		{
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				// If there was an error loading a type from the assembly, only return the types 
				// that were actually loaded successfully
				return ex.Types.Where(type => type != null);
			}
		}

		/// <summary>
		/// Gets a mapping of  namespace prefix to area name
		/// </summary>
		/// <returns></returns>
		private IDictionary<string, string> GetAreaNamespaceMap()
		{
			var mapping = new Dictionary<string, string>();
			
			foreach (var route in _routeCollection.GetRoutes().OfType<Route>())
			{
				// Skip routes with no area or namespace
				if (route.DataTokens == null || route.DataTokens["area"] == null || route.DataTokens["Namespaces"] == null)
					continue;

				var area = (string)route.DataTokens["area"];
				var namespaces = (IList<string>)route.DataTokens["Namespaces"];
				foreach (var ns in namespaces)
				{
					// MVC adds namespaces in the format "Daniel15.Areas.Blah.*", but we don't 
					// care about the asterisk.
					mapping[ns.TrimEnd('*')] = area;
				}
			}

			return mapping;
		}

		/// <summary>
		/// Returns all the controllers that implement the specified attribute
		/// </summary>
		/// <param name="types">The controller types.</param>
		/// <param name="attributeType">Type of the attribute to check for.</param>
		/// <returns>All the types that implement the specified attribute</returns>
		private Dictionary<string, Type> ControllersImplementingAttribute(IEnumerable<Type> types, Type attributeType)
		{
			return types
				.Where(c => c.IsDefined(attributeType, true))
				// Remove "Controller" from the class name as it's not used when referencing the controller
				.ToDictionary(c =>  c.Name.Replace("Controller", string.Empty), c => c);
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

			if (route.Defaults == null)
				return true;

			// If there's no controller specified, we need to check if it's in an area
			if (!route.Defaults.ContainsKey("controller"))
			{
				// Not an area, so it's a "regular" default route
				if (route.DataTokens == null || !route.DataTokens.ContainsKey("area"))
					return true;

				// Exposing all routes, or an area that's explicitly whitelisted
				if (_configuration.ExposeAllRoutes || _areaWhitelist.Contains(route.DataTokens["area"]))
					return true;

				// In an area that's not exposed, so this route shouldn't be exposed.
				return false;
			}

			var controller = route.Defaults["controller"].ToString();

			// If explicitly blacklisted, always deny
			if (_controllerBlacklist.Keys.Contains(controller))
				return false;

			// If explicitly whitelisted, always allow
			if (_controllerWhitelist.Keys.Contains(controller))
				return true;

			// Otherwise, allow based on configuration
			return _configuration.ExposeAllRoutes;
		}
	}
}
