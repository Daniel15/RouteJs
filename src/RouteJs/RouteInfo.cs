using System.Collections.Generic;
using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Information about a route
	/// </summary>
	public class RouteInfo
	{
		/// <summary>
		/// URL of the route
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// Default values for the route
		/// </summary>
		public RouteValueDictionary Defaults { get; set; }
		/// <summary>
		/// Constraints imposed on the route
		/// </summary>
		public RouteValueDictionary Constraints { get; set; }
		/// <summary>
		/// Any optional parameters in the URL
		/// </summary>
		public IList<string> Optional { get; set; }
	}
}
