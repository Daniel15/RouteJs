using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace RouteJs
{
    public class DefaultRouteFilter : IRouteFilter
    {
		/// <summary>
		/// RouteJs configuration
		/// </summary>
		private readonly IRouteJsConfiguration _configuration;
		/// <summary>
		/// Action descriptors collection, containing information about all actions.
		/// </summary>
		private readonly IActionDescriptorCollectionProvider _actionDescriptorsCollectionProvider;

		/// <summary>
		/// Whitelist of controllers whose routes are always rendered.
		/// </summary>
		private readonly ISet<string> _controllerWhitelist = new HashSet<string>();
		/// <summary>
		/// Blacklist of controllers whose routes are never rendered.
		/// </summary>
		private readonly ISet<string> _controllerBlacklist = new HashSet<string>();

	    public DefaultRouteFilter(IRouteJsConfiguration configuration, IActionDescriptorCollectionProvider actionDescriptorsCollectionProvider)
	    {
		    _configuration = configuration;
		    _actionDescriptorsCollectionProvider = actionDescriptorsCollectionProvider;
		    BuildLists();
	    }

		/// <summary>
		/// Build the controller whitelist and blacklist.
		/// </summary>
	    private void BuildLists()
		{
			var actions = _actionDescriptorsCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>();

			var processedControllers = new HashSet<string>();
			foreach (var action in actions)
			{
				if (processedControllers.Contains(action.ControllerName))
				{
					continue;
				}

				var attribute = action.ControllerTypeInfo.CustomAttributes.FirstOrDefault(x => 
					x.AttributeType == typeof(ExposeRoutesInJavaScriptAttribute) ||
					x.AttributeType == typeof(HideRoutesInJavaScriptAttribute)
				);
				if (attribute?.AttributeType == typeof (ExposeRoutesInJavaScriptAttribute))
				{
					_controllerWhitelist.Add(action.ControllerName);
				}
				else if (attribute?.AttributeType == typeof (HideRoutesInJavaScriptAttribute))
				{
					_controllerBlacklist.Add(action.ControllerName);
				}
				processedControllers.Add(action.ControllerName);
			}
		}

	    /// <summary>
	    /// Check whether the specified route should be exposed in the JavaScript output
	    /// </summary>
	    /// <param name="routeInfo">Route to check</param>
	    /// <returns><c>false</c> if the route should definitely be blocked, <c>true</c> if the route should be exposed (or unsure)</returns>
	    public bool AllowRoute(RouteInfo routeInfo)
	    {
			// No controller name, no idea what to do with it.
			// Let another filter take care of this
		    if (!routeInfo.Defaults.ContainsKey("controller"))
		    {
			    return true;
		    }

		    var controller = (string)routeInfo.Defaults["controller"];
		    // If explicitly blacklisted, always deny
		    if (_controllerBlacklist.Contains(controller))
		    {
			    return false;
		    }
			// If explicitly whitelisted, always allow
		    if (_controllerWhitelist.Contains(controller))
		    {
			    return true;
		    }

			// Otherwise, allow based on configuration
			return _configuration.ExposeAllRoutes;
		}
	}
}
