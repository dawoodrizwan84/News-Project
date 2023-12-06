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
using Microsoft.AspNetCore.Identity;
using _23._1News.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace _23._1News.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        private readonly IWeatherService _weatherService;
        private readonly IWeatherService? weatherService;
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly IEmailHelper _emailHelper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;


        public HomeController(ILogger<HomeController> logger,
          IArticleService articleService,
          ICategoryService categoryService,
          IEmailConfiguration emailConfiguration,
          UserManager<User> userManager,
          IEmailHelper emailHelper,
          IEmailSender emailSender
          )
        {
            _logger = logger;
            _articleService = articleService;
            _weatherService = weatherService;
            _categoryService = categoryService;
            _emailConfiguration = emailConfiguration;
            _emailHelper = emailHelper;
            _userManager = userManager;
            _emailSender = emailSender;
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