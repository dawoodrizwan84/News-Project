
using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.View_Models;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace _23._1News.Controllers
{
    public class ArticleController : Controller
    {

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IArticleService _articleService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger, 
                IArticleService articleService, 
                ApplicationDbContext applicationDbContext,
                UserManager<User> userManager)
        {
            _logger = logger;
            _articleService = articleService;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;

        }
        [Route("Ai")]
        public IActionResult Index()
        {
            var articleList = _articleService.GetArticles();
            return View(articleList);
        }

        [Route("cr")]
        [Authorize(Roles = "Editor")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("cr")]
        [HttpPost]
        [Authorize(Roles = "Editor")]
        public IActionResult Create(ArticleVM articleVM)
        {
            var userId = _userManager.GetUserId(User);
            _articleService.CreateArticle(articleVM, userId);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var record = _articleService.GetArticleById(id);
            return View(record);
        }

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
            TempData["msg"] = "Error has occurred on the server side.";
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

    }
}
