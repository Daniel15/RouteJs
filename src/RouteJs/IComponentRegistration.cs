using RouteJs.TinyIoc;

namespace RouteJs
{
	/// <summary>
	/// IoC component registration. Used to register components in the RouteJs IoC container. Every
	/// RouteJs assembly should have an instance of IComponentRegistration.
	/// </summary>
	public interface IComponentRegistration
	{
		/// <summary>
		/// Registers components in the RouteJs IoC container
		/// </summary>
		/// <param name="container">Container to register components in</param>
		void Register(TinyIoCContainer container);
	}
}
