using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace RouteJs
{
	/// <summary>
	/// Handles processing of route constraints, including filtering of unsupported constraints.
	/// </summary>
	public interface IConstraintsProcessor
	{
		/// <summary>
		/// Processes the specified constraints
		/// </summary>
		/// <param name="constraints">Constraints to process</param>
		/// <returns>Constraints in a format usable from JavaScript</returns>
		IDictionary<string, object> ProcessConstraints(IEnumerable<KeyValuePair<string, IRouteConstraint>> constraints);
	}
}
