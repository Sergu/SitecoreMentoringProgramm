using Glass.Mapper.Sc.Web.Mvc;
using System.Web.Mvc;

namespace OutOfWebrotApp.Controllers.Pages
{
    public class HomeController : GlassController
    {
        // GET: Home
        public ActionResult GetHomePage()
        {
            return View("~/Views/Pages/Home/Home.cshtml");
        }
    }
}