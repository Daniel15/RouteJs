using Microsoft.AspNetCore.Mvc.Rendering;

namespace RouteJs
{
	/// <summary>
	/// RouteJs ASP.NET MVC helper. Handles rendering scripts tag to load the RouteJs script.
	/// </summary>
	public interface IRouteJsHelper
	{
		/// <summary>
		/// Renders the RouteJs script tag.
		/// </summary>
		/// <returns>A script tag to load RouteJs</returns>
		HtmlString Render();
	}
}
