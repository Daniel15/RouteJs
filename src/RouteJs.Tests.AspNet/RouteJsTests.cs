using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Routing;
using Moq;
using Xunit;

namespace RouteJs.Tests.AspNet
{
	/// <summary>
	/// Unit tests for <see cref="RouteJs" />.
	/// </summary>
    public class RouteJsTests
    {
		[Fact]
		public void GetsRoutesFromRouteFetcher()
		{
			var routes = new[]
			{
				new RouteInfo
				{
					Constraints = new Dictionary<string, object> {{"constr", "aint"}},
					Defaults = new Dictionary<string, object>
					{
						{"controller", "Foo"},
						{"action", "Bar"}
					},
					Optional = new[] {"id"},
					Url = "foo/bar",
				},
			};
			var routeFetcher = new Mock<IRouteFetcher>();
			routeFetcher.Setup(x => x.GetRoutes(It.IsAny<RouteData>())).Returns(routes);

			var request = new Mock<HttpRequest>();
			request.Setup(x => x.PathBase).Returns(new PathString("/base"));
			var httpContext = new Mock<HttpContext>();
			httpContext.Setup(x => x.Request).Returns(request.Object);
			var actionContext = new ScopedInstance<ActionContext>()
			{
				Value = new ActionContext
				{
					HttpContext = httpContext.Object
				}
			};

			var routeJs = new RouteJs(new[] { routeFetcher.Object }, actionContext, Enumerable.Empty<IRouteFilter>());
			var result = routeJs.GetJsonData();
			const string expected = @"{""routes"":[{""url"":""foo/bar"",""defaults"":{""controller"":""Foo"",""action"":""Bar""},""constraints"":{""constr"":""aint""},""optional"":[""id""]}],""baseUrl"":""/base""}";
			Assert.Equal(expected, result);
		}
    }
}
