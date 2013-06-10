using System.Web.Mvc;

namespace RouteJs.Tests.Mvc.Controllers
{
	[ExposeRoutesInJavaScript]
	public partial class HelloExposedController : Controller
	{
	}

	[HideRoutesInJavaScript]
	public partial class HelloHiddenController : Controller
	{
	}

	public partial class HelloController : Controller
	{
		public virtual ActionResult HelloWorld()
		{
			return Content("Hello world");
		}
	}
}
