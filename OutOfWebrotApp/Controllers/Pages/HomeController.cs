	using System.Web.Mvc;

namespace OutOfWebrotApp.Controllers.Pages
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View("~/Views/Pages/Home/Home.cshtml");
        }
    }
}