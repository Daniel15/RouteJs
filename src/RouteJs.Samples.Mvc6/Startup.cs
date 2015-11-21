using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace RouteJs.Samples.Mvc6
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();

			services.AddRouteJs().ConfigureRouteJs(config =>
			{
				config.ExposeAllRoutes = true;
			});
		}

        public void Configure(IApplicationBuilder app)
        {
			// Add the platform handler to the request pipeline.
			app.UseIISPlatformHandler();

			app.UseStaticFiles();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "Hello",
					template: "hello/world",
					defaults: new { controller = "Home", action = "HelloWorld" }
				);

				routes.MapRoute(
					name: "HelloBool",
					template: "bool/{message:bool}",
					defaults: new { controller = "Home", action = "HelloWorld2" }
				);

				routes.MapRoute(
					name: "RegexConstraints",
					template: "regex/{message:regex(^\\d+$)}",
					defaults: new { controller = "Home", action = "HelloWorld2" }
				);

				routes.MapRoute(
					name: "Hello2",
					template: "hello/mvc/{message}",
					defaults: new { controller = "Home", action = "HelloWorld2" }
				);

				routes.MapRoute(
					name: "Hidden",
					template: "hidden/three",
					defaults: new { controller = "Hidden", action = "Index" }
				);

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}"
				);
			});
        }
    }
}
