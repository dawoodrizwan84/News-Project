using _23._1News.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using _23._1News.Services;
using _23._1News.Services.Abstract;
using _23._1News.Services.Implement;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using _23._1News.Models.Email;
using _23._1News.Helpers;
using _23._1News.Models.Db;

namespace _23._1News.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        private readonly IWeatherService _weatherService;
        private readonly IWeatherService? weatherService;


        public HomeController(ILogger<HomeController> logger,
          IArticleService articleService,
          ICategoryService categoryService
          )
        {
            _logger = logger;
            _articleService = articleService;
            _weatherService = weatherService;
            _categoryService = categoryService;
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

        public IActionResult LatestVC(string count)
        {

            return ViewComponent("LatestNews", new {count = count});
        }

        public IActionResult EditorsVC(string count)
        {

            return ViewComponent("EditorsChoice", new { count = count });
        }

        public IActionResult News(int id)
        {
         
            var articles=_articleService.GetArticles(id);
            Category category = _categoryService.GetCategotyById(id);
            //TempData["Headline"] = category.Name;
            return View(articles);
        }


        public IActionResult World()
        {
            return View();
        }


    }
}