using System.Web;
using Moq;
using NUnit.Framework;

namespace RouteJs.Tests
{
	[TestFixture]
	public class RouteJsHandlerTests
	{
		[TestCase("", true, false, false)]
		[TestCase("12345/router.min.js", false, true, false)]
		[TestCase("12345/router.js", true, true, false)]
		[TestCase("12345/invalid.js", true, false, true)]
		public void CheckDebugModeTests(string pathInfo, bool expectedDebugMode, bool expectedCacheHeaders, bool expectedFileNotFound)
		{
			var request = new Mock<HttpRequestBase>();
			request.Setup(x => x.PathInfo).Returns(pathInfo);

			bool fileNotFound;
			bool cacheHeaders;
			bool debugMode = RouteJsHandler.CheckDebugMode(request.Object, out cacheHeaders, out fileNotFound);

			Assert.AreEqual(expectedDebugMode, debugMode);
			Assert.AreEqual(expectedCacheHeaders, cacheHeaders);
			Assert.AreEqual(expectedFileNotFound, fileNotFound);
		}
	}
}
