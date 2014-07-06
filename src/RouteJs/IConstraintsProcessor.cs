using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Handles processing of route constraints, including filtering of unsupported constraints.
	/// </summary>
	public interface IConstraintsProcessor
	{
		/// <summary>
		/// Process the constraints of the specified route
		/// </summary>
		/// <param name="constraints">Constraints to process</param>
		/// <param name="routeInfo">Output information about the route</param>
		void ProcessConstraints(RouteValueDictionary constraints, RouteInfo routeInfo);
	}
}
