using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using RouteJs.AspNet;

namespace RouteJs.Samples.Mvc6
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();

			services.AddRouteJs();
		}

        public void Configure(IApplicationBuilder app)
        {
			app.UseStaticFiles();
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "Hello",
					template: "hello/world",
					defaults: new { controller = "Home", action = "HelloWorld" }
				);

				routes.MapRoute(
					name: "Hello2",
					template: "hello/mvc/{message}",
					defaults: new { controller = "Home", action = "HelloWorld2" }
				);

				routes.MapRoute(
					name: "RegexConstraints",
					template: "regex/{message:regex(^\\d+$)}",
					defaults: new { controller = "Home", action = "HelloWorld2" }
				);

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}"
				);
			});
        }
    }
}
