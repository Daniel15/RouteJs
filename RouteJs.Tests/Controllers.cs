using System.Web.Mvc;

namespace RouteJs.Tests
{
	[ExposeRoutesInJavaScript]
	public class HelloExposedController : Controller
	{
	}

	[HideRoutesInJavaScript]
	public class HelloHiddenController : Controller
	{
	}

	public class HelloController : Controller
	{
	}
}
