using Microsoft.AspNetCore.Routing.Template;

namespace RouteJs
{
	/// <summary>
	/// Parses useful information out of a route template
	/// </summary>
	public interface IRouteTemplateParser
	{
		/// <summary>
		/// Parses the specified route template string, placing the available information in
		/// <paramref name="info"/>.
		/// </summary>
		/// <param name="template">Template to parse</param>
		/// <param name="info">RouteInfo to store parsed info</param>
		void Parse(RouteTemplate template, RouteInfo info);
	}
}
