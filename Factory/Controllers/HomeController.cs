using Microsoft.AspNetCore.Mvc;

namespace Facory.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }

    }
}