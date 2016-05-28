using System.Text;
using Microsoft.AspNetCore.Routing.Template;

namespace RouteJs
{
	/// <summary>
	/// Parses useful information out of a route template
	/// </summary>
	public class RouteTemplateParser : IRouteTemplateParser
	{
		/// <summary>
		/// Parses the specified route template string, placing the available information in
		/// <paramref name="info" />.
		/// </summary>
		/// <param name="template">Template to parse</param>
		/// <param name="info">RouteInfo to store parsed info</param>
		public void Parse(RouteTemplate template, RouteInfo info)
		{
			var builder = new StringBuilder();
			foreach (var segment in template.Segments)
			{
				foreach (var part in segment.Parts)
				{
					if (part.IsLiteral)
					{
						builder.Append(part.Text);
						continue;
					}
					if (part.DefaultValue != null)
					{
						info.Defaults[part.Name] = part.DefaultValue;
					}
					if (part.IsOptional)
					{
						info.Optional.Add(part.Name);
					}
					builder.Append('{');
					builder.Append(part.Name);
					builder.Append('}');

					// TODO part.IsOptionalSeperator
					// TODO part.IsCatchAll
				}
				builder.Append('/');
			}
			info.Url = builder.ToString().TrimEnd('/');
		}
	}
}
