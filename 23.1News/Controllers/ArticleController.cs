using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Local()
        {
            return View();
        }

        public IActionResult Sweden()
        {
            return View();
        }

        public IActionResult World()
        {
            return View();
        }

        public IActionResult Weather()
        {
            return View();
        }

        public IActionResult Economy()
        {
            return View();
        }

        public IActionResult Sport()
        {
            return View();
        }
    }
}
