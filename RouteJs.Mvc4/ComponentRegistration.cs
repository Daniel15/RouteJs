namespace RouteJs.Mvc4
{
	/// <summary>
	/// Register all RouteJs components that are coupled with ASP.NET MVC
	/// </summary>
	public class ComponentRegistration : IComponentRegistration
	{
		/// <summary>
		/// Registers components in the RouteJs IoC container
		/// </summary>
		/// <param name="container">Container to register components in</param>
		public void Register(TinyIoCContainer container)
		{
			container.Register<IRouteFilter, MvcRouteFilter>("MvcRouteFilter");
		}
	}
}
