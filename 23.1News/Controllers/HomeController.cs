using _23._1News.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using _23._1News.Services;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace _23._1News.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;

        public HomeController(ILogger<HomeController> logger,
          IArticleService articleService
          )
        {
            _logger = logger;
            _articleService = articleService;
        }
       
        public IActionResult Index()
        {
            var articleList = _articleService.GetArticles();

            return View(articleList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Local()
        {
            return View();
        }
        public IActionResult News(int id)
        {
            var articles=_articleService.GetArticles(id);
            return View(articles);
        }


        public IActionResult World()
        {
            return View();
        }
    }
}