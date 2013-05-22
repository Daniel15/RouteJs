using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Handles processing of defaults for route parameters
	/// </summary>
	public interface IDefaultsProcessor
	{
		/// <summary>
		/// Process the defaults of the specified route
		/// </summary>
		/// <param name="route">Route to process</param>
		/// <param name="routeInfo">Output information about the route</param>
		void ProcessDefaults(Route route, RouteInfo routeInfo);
	}
}
