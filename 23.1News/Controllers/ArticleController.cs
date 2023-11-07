
using _23._1News.Data;
using _23._1News.Data.Migrations;
using _23._1News.Models.Db;
using _23._1News.Models.View_Models;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Web;
using Microsoft.EntityFrameworkCore;
using _23._1News.Services.Implement;


namespace _23._1News.Controllers
{
    public class ArticleController : Controller
    {

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IArticleService _articleService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ArticleController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IWeatherService _weatherService;

        public ArticleController(ILogger<ArticleController> logger,
                IArticleService articleService,
                IWeatherService weatherService,
                ApplicationDbContext applicationDbContext,
                UserManager<User> userManager,
                IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _articleService = articleService;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _weatherService = weatherService;
        }


        [Route("Ai")]
        [Authorize(Roles = "Editor, Admin")]
        public IActionResult Index()
        {
            var articleList = _articleService.GetArticles();
            return View(articleList);
        }


      
        //[Authorize(Roles = "Editor, Admin")]

        public IActionResult Create()
        {
            ArticleVM addArticle = new ArticleVM();

            var categories = _articleService.GetCategories();

            foreach (var category in categories)
            {
                addArticle.Categories.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name
                });
            }
            return View(addArticle);
        }




     
        [HttpPost]
        //[Authorize(Roles = "Editor , Admin")]

        public IActionResult Create(ArticleVM articleVM)
        {

            //if (articleVM.File != null && articleVM.File.Length > 0)
            //{

            //    string uniqueFileName = Guid.NewGuid().ToString() + "_" + articleVM.File.FileName;
            //    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image");
            //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        articleVM.File.CopyTo(fileStream);
            //    }


            //    articleVM.ImageLink = "/image/" + uniqueFileName;
            //}


            articleVM.ImageLink = _articleService.UploadImageFile(articleVM.File);

            var userId = _userManager.GetUserId(User);
            _articleService.CreateArticle(articleVM, userId);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Editor, Admin")]
        public IActionResult Edit(int id)
        {
            var record = _articleService.GetArticleById(id);
            return View(record);
        }

        [Authorize(Roles = "Editor, Admin")]
        [HttpPost]
        public IActionResult Edit(ArticleVM newArticle)
        {
            if (!ModelState.IsValid)
            {
                return View(newArticle);
            }
            var result = _articleService.UpdateArticle(newArticle);
            if (result)
            {
                return RedirectToAction("Index");
            }

            return View(newArticle);
        }

        public IActionResult Delete(int id)
        {
            var del = _articleService.DeleteArticle(id);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var det = _articleService.GetArticleById(id);

            if (det == null)
            {
                return NotFound();
            }
            return View(det);
        }


        public IActionResult Search()
        {
            string Headline = Request.Query["search"];
            var SearchArticles = _articleService.SearchArticle(Headline);

            if (SearchArticles == null)
            {
                return NotFound();
            }

            return View(SearchArticles);
        }
        public IActionResult GetWeatherForecast()
        {
            //var weatherForecast = _weatherService.GetWeatherForecast("linköping").Result;
            var weatherForecast = _weatherService.GetWeatherForecast("Linköping").Result;

            return View(weatherForecast);
        }
    }
}
