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

namespace RouteJs.Tests.Mvc.Areas.UndecoratedArea.Controllers
{
	public partial class UndecoratedAreaHelloController : Controller
	{
	}
}

namespace RouteJs.Tests.Mvc.Areas.ExposedArea.Controllers
{
	[ExposeRoutesInJavaScript]
	public partial class ExposedAreaHelloController : Controller
	{
	}
}

namespace RouteJs.Tests.Mvc.Areas.HiddenArea.Controllers
{
	[HideRoutesInJavaScript]
	public partial class HiddenAreaHelloController : Controller
	{
	}
}
