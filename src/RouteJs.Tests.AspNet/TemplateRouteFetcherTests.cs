using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Routing;
using Moq;
using Xunit;

namespace RouteJs.Tests.AspNet
{
    public class TemplateRouteFetcherTests
    {
		[Fact]
		public void SimpleLiteralRoute()
		{
			var result = RunFetcher(routes =>
			{
				routes.MapRoute(
					name: "Hello",
					template: "hello/world",
					defaults: new { controller = "Hello", action = "HelloWorld" }
				);
			});
			Assert.Equal(1, result.Count);
			Assert.Equal("hello/world", result[0].Url);
			Assert.Equal("Hello", result[0].Defaults["controller"]);
			Assert.Equal("HelloWorld", result[0].Defaults["action"]);
		}

		[Fact]
	    public void RouteWithDefaults()
	    {
			var result = RunFetcher(routes =>
			{
				routes.MapRoute(
					name: "Hello",
					template: "hello/{action=World}/{id=123}",
					defaults: new { controller = "Hello" }
				);
			});
			Assert.Equal(1, result.Count);
			Assert.Equal("hello/{action}/{id}", result[0].Url);
			Assert.Equal("Hello", result[0].Defaults["controller"]);
			Assert.Equal("World", result[0].Defaults["action"]);
			Assert.Equal("123", result[0].Defaults["id"]);
		}

		[Fact]
	    public void RouteWithOptional()
	    {
			var result = RunFetcher(routes =>
			{
				routes.MapRoute(
					name: "Hello",
					template: "hello/world/{id?}",
					defaults: new { controller = "Hello", action = "HelloWorld" }
				);
			});
			Assert.Equal(1, result.Count);
			Assert.Equal("hello/world/{id}", result[0].Url);
			Assert.Equal("Hello", result[0].Defaults["controller"]);
			Assert.Equal("HelloWorld", result[0].Defaults["action"]);
			Assert.Equal(new List<string> {"id"}, result[0].Optional);
		}

		[Fact]
		public void RouteWithMultiplePartsInSegment()
		{
			var result = RunFetcher(routes =>
			{
				routes.MapRoute(
					name: "Hello",
					template: "controller-{controller=Hello}/action-{action=World}"
				);
			});
			Assert.Equal(1, result.Count);
			Assert.Equal("controller-{controller}/action-{action}", result[0].Url);
			Assert.Equal("Hello", result[0].Defaults["controller"]);
			Assert.Equal("World", result[0].Defaults["action"]);
		}

		/// <summary>
		/// Runs the <see cref="TemplateRouteFetcher"/> with the specified route configuration.
		/// </summary>
		/// <param name="configure">Function to configure routes</param>
		/// <returns>List of routes returned from the route fetcher</returns>
		private static IList<RouteInfo> RunFetcher(Action<RouteBuilder> configure)
	    {
			var routes = new RouteBuilder();
			var serviceProvider = new Mock<IServiceProvider>();
			serviceProvider.Setup(x => x.GetService(typeof(IInlineConstraintResolver))).Returns(
				new Mock<IInlineConstraintResolver>().Object
			);
			routes.ServiceProvider = serviceProvider.Object;
			routes.DefaultHandler = new Mock<IRouter>().Object;
		    configure(routes);

			var router = routes.Build();
			var routeData = new RouteData();
			routeData.Routers.Add(router);

			var fetcher = new TemplateRouteFetcher();
			return fetcher.GetRoutes(routeData).ToList();
		}
    }
}
