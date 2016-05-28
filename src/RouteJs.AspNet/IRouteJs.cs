namespace RouteJs
{
	/// <summary>
	/// Main interface for RouteJs. 
	/// </summary>
	public interface IRouteJs
	{
		/// <summary>
		/// Gets the JavaScript routes
		/// </summary>
		/// <returns>JavaScript for the routes</returns>
		string GetJavaScript(bool debugMode);

		/// <summary>
		/// Gets the JSON data representing the routes
		/// </summary>
		/// <returns>JavaScript for the routes</returns>
		string GetJsonData();
	}
}
