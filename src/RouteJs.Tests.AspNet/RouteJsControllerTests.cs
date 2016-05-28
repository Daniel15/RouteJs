using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace RouteJs.Tests.AspNet
{
	/// <summary>
	/// Unit tests for <see cref="RouteJsController" />
	/// </summary>
    public class RouteJsControllerTests
    {
	    [Fact]
	    public void BasicTest()
	    {
		    var controller = CreateController();
		    var result = (ContentResult)controller.Routes(hash: null, environment: null);

			Assert.Equal("SampleDebug", result.Content);
			Assert.Equal("text/javascript", result.ContentType.ToString());
	    }

		[Fact]
	    public void MinifiedScriptReturnedWhenUsingMinEnvironment()
	    {
			var controller = CreateController();
			var result = (ContentResult)controller.Routes(hash: null, environment: "min");

			Assert.Equal("SampleProd", result.Content);
		}

	    [Fact]
	    public void CachesWhenHashIsPassed()
	    {
			var controller = CreateController();
			var result = (ContentResult)controller.Routes(hash: "yo", environment: null);

		    var headers = controller.Response.Headers;
			Assert.Contains("Expires", headers.Keys);
			Assert.Equal("public, max-age=31536000", headers["Cache-control"]);
		}

		[Fact]
	    public void DoesNotCacheWhenHashIsNotPassed()
	    {
			var controller = CreateController();
			var result = (ContentResult)controller.Routes(hash: null, environment: null);
			var headers = controller.Response.Headers;
			Assert.DoesNotContain("Expires", headers.Keys);
			Assert.DoesNotContain("Cache-control", headers.Keys);
		}

	    private static RouteJsController CreateController()
	    {
			// Mock Response.Headers
			var headers = new HeaderDictionary();
			var response = new Mock<HttpResponse>();
			response.Setup(x => x.Headers).Returns(headers);
			var httpContext = new Mock<HttpContext>();
			httpContext.Setup(x => x.Response).Returns(response.Object);

			// Mock RouteJs
			var routeJs = new Mock<IRouteJs>();
			routeJs.Setup(x => x.GetJavaScript(true)).Returns("SampleDebug");
			routeJs.Setup(x => x.GetJavaScript(false)).Returns("SampleProd");

		    return new RouteJsController(routeJs.Object)
		    {
			    ControllerContext = new ControllerContext { HttpContext = httpContext.Object}
		    };
	    }
    }
}
