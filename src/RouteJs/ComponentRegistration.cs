using System;
using System.Configuration;
using System.Linq;
using System.Web.Routing;

namespace RouteJs
{
	/// <summary>
	/// Registers all standard RouteJs classes in IoC
	/// </summary>
	public class ComponentRegistration : IComponentRegistration
	{
		/// <summary>
		/// Registers components in the RouteJs IoC container
		/// </summary>
		/// <param name="container">Container to register components in</param>
		public void Register(TinyIoCContainer container)
		{
			container.Register<IConfiguration>((c, o) => (RouteJsConfigurationSection)ConfigurationManager.GetSection("routeJs"));
			container.Register<RouteCollection>((c, o) => RouteTable.Routes);
		}

		/// <summary>
		/// Initialises the IoC container by finding all <see cref="IComponentRegistration"/> 
		/// implementations and calling their <see cref="IComponentRegistration.Register"/> methods.
		/// </summary>
		/// <param name="container">Container to register components in</param>
		public static void RegisterAll(TinyIoCContainer container)
		{
			var types = AppDomain.CurrentDomain.GetAssemblies()
				// Only bother checking RouteJs assemblies
				.Where(assembly => assembly.FullName.StartsWith("RouteJs"))
				.SelectMany(assembly => assembly.GetTypes())
				.Where(IsComponentRegistration);

			foreach (var type in types)
			{
				var reg = (IComponentRegistration) Activator.CreateInstance(type);
				reg.Register(container);
			}
		}

		/// <summary>
		/// Determines whether the specified type is a component registration class
		/// </summary>
		/// <param name="type">The type to check</param>
		/// <returns>
		///   <c>true</c> if the type is a component registration class; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsComponentRegistration(Type type)
		{
			return type.GetInterfaces().Contains(typeof (IComponentRegistration));
		}
	}
}
