using Microsoft.AspNet.Mvc;

namespace RouteJs.Samples.Mvc6.Controllers
{
	[HideRoutesInJavaScript]
    public class HiddenController : Controller
    {
		[Route("hidden/one")]
        public IActionResult Index()
        {
            return Content("I should not be in JS output");
        }

		[Route("hidden/two")]
		public IActionResult Second()
		{
			return Content("I should not be in JS output 2");
		}
	}
}
