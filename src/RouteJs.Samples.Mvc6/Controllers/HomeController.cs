using Microsoft.AspNet.Mvc;

namespace RouteJs.Samples.Mvc6.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult HelloWorld()
		{
			return Content("Hello MVC6 world");
		}

		public IActionResult HelloWorld2(string message)
		{
			return Content("Hello MVC6 world - Message = " + message);
		}

		[Route("hello/from/attribute/{message}")]
		[HttpGet("hello/from/getattr/{message}")]
		[Route("bool/attribute/{message:bool}")]
		public IActionResult HelloFromAttribute(string message)
		{
			return Content("Hello from attribute!");
		}
	}
}
