namespace RouteJs.Mvc4
{
	public class ComponentRegistration : IComponentRegistration
	{
		public void Register(TinyIoCContainer container)
		{
			container.Register<IRouteFilter, MvcRouteFilter>();
		}
	}
}
