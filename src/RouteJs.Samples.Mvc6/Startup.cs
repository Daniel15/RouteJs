using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace RouteJs.Samples.Mvc6
{
    public class Startup
    {
		public static void Main(string[] args)
		{
			var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}

		public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

			services.AddRouteJs(config =>
			{
				config.ExposeAllRoutes = true;
			});
		}

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
			loggerFactory.AddConsole();
			loggerFactory.AddDebug();

	        if (env.IsDevelopment())
	        {
		        app.UseDeveloperExceptionPage();
	        }

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
