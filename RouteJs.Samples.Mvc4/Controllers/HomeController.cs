using System.Web.Mvc;

namespace RouteJs.Samples.Mvc4.Controllers
{
    public class HomeController : Controller
    {
		public ActionResult Index()
		{
			return View();
		}

        public ActionResult HelloWorld()
        {
	        return Content("Hello MVC4 world");
        }

		public ActionResult HelloWorld2(string message)
		{
			return Content("Hello MVC4 world - Message = " + Server.HtmlEncode(message));
		}
    }
}
