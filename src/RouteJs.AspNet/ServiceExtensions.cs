using System;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.OptionsModel;

namespace RouteJs
{
	/// <summary>
	/// RouteJs extensions to <see cref="IServiceCollection"/>.
	/// </summary>
    public static class ServiceExtensions
    {
		/// <summary>
		/// Installs RouteJs services for this website.
		/// </summary>
		/// <param name="services">Service collection to register routes in</param>
		/// <returns>The service collection</returns>
	    public static IServiceCollection AddRouteJs(this IServiceCollection services)
	    {
			services.AddSingleton<IRouteJsConfiguration>(provider => provider.GetRequiredService<IOptions<RouteJsConfiguration>>().Options);
			services.AddSingleton<IRouteFetcher, TemplateRouteFetcher>();
		    services.AddSingleton<IRouteFetcher, AttributeRouteFetcher>();
			services.AddSingleton<IConstraintsProcessor, ConstraintsProcessor>();
			services.AddSingleton<IRouteTemplateParser, RouteTemplateParser>();
			services.AddSingleton<IRouteFilter, DefaultRouteFilter>();
			services.AddScoped<IRouteJsHelper, RouteJsHelper>();
			services.AddScoped<IRouteJs, RouteJs>();
			
		    return services;
	    }

		/// <summary>
		/// Configures RouteJs for this site.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configure"></param>
		/// <returns></returns>
		public static IServiceCollection ConfigureRouteJs(this IServiceCollection services, Action<RouteJsConfiguration> configure)
		{
			return services.Configure(configure);
		}
    }
}
