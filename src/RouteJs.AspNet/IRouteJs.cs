namespace RouteJs
{
	public interface IRouteJs
	{
		/// <summary>
		/// Gets the JavaScript routes
		/// </summary>
		/// <returns>JavaScript for the routes</returns>
		string GetJavaScript(bool debugMode);
	}
}
