using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleService _articleService;
        public ArticleController(ILogger<ArticleController> logger, IArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
