namespace RouteJs
{
	/// <summary>
	/// RouteJs configuration
	/// </summary>
	public class Configuration : IConfiguration
    {
		public Configuration()
		{
			ExposeAllRoutes = true;
		}

	    /// <summary>
	    /// Gets whether to expose all routes to the site. 
	    /// If <c>true</c>, all routes will be exposed unless explicitly hidden using <see cref="HideRoutesInJavaScriptAttribute"/>.
	    /// If <c>false</c>, all routes will be hidden unless explicitly exposed using <see cref="ExposeRoutesInJavaScriptAttribute"/>.
	    /// </summary>
	    public bool ExposeAllRoutes { get; set; }
    }
}
