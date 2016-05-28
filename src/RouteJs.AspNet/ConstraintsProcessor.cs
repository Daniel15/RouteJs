using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;

namespace RouteJs
{
	/// <summary>
	/// Handles processing of route constraints, including filtering of unsupported constraints.
	/// </summary>
	public class ConstraintsProcessor : IConstraintsProcessor
	{
		/// <summary>
		/// Processes the specified constraints
		/// </summary>
		/// <param name="constraints">Constraints to process</param>
		/// <returns>Constraints in a format usable from JavaScript</returns>
		public IDictionary<string, object> ProcessConstraints(IEnumerable<KeyValuePair<string, IRouteConstraint>> constraints)
		{
			var output = new Dictionary<string, object>();

			foreach (var kvp in constraints)
			{
				var regex = GetRegexForConstraint(kvp.Value);
				if (regex != null)
				{
					output.Add(kvp.Key, regex);
				}
			}
			return output;
		}

		/// <summary>
		/// Gets a regular expression that handles the specified constraint type
		/// </summary>
		/// <param name="constraint"></param>
		/// <returns></returns>
		private static string GetRegexForConstraint(IRouteConstraint constraint)
		{
			if (constraint is RegexRouteConstraint)
			{
				return ((RegexRouteConstraint) constraint).Constraint.ToString();
			}
			if (constraint is BoolRouteConstraint)
			{
				return @"^(True|False)$";
			}
			if (
				constraint is DecimalRouteConstraint || 
				constraint is DoubleRouteConstraint || 
				constraint is FloatRouteConstraint ||
				constraint is LongRouteConstraint
			)
			{
				return @"^[+\-]?^\d+\.?\d+$";
			}
			if (constraint is GuidRouteConstraint)
			{
				return "^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$";
			}
			if (constraint is IntRouteConstraint)
			{
				return @"^[+\-]?\d+$";
			}
			return null;
		}
	}
}
