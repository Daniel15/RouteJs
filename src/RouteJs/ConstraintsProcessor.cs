using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Handles processing of route constraints, including filtering of unsupported constraints.
	/// </summary>
	public class ConstraintsProcessor : IConstraintsProcessor
	{
		/// <summary>
		/// Process the constraints of the specified route
		/// </summary>
		/// <param name="constraints">Constraints to process</param>
		/// <param name="routeInfo">Output information about the route</param>
		public void ProcessConstraints(RouteValueDictionary constraints, RouteInfo routeInfo)
		{
			Contract.Requires(routeInfo.Constraints != null);

			// Only add strings to the route info
			var supportedConstraints = constraints.Where(constraint => constraint.Value is string);
			foreach (var kvp in supportedConstraints)
			{
				routeInfo.Constraints.Add(kvp.Key, kvp.Value);
			}
		}
	}
}
