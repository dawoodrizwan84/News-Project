using _23._1News.Data;
using _23._1News.Models;
using _23._1News.Models.View_Models;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace _23._1News.Controllers
{
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IAdminService _adminService;
        private readonly IArticleService _articleService;

        public AdminController(ILogger<AdminController> logger,
            IArticleService articleService, IAdminService adminService,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _adminService = adminService;
            _applicationDbContext = applicationDbContext;
            _articleService = articleService;

        }

        [Authorize(Roles = "Editor, Admin")]
        public IActionResult Index()
        {


            var articleList = _articleService.GetArticles();
            return View(articleList);

        }
       
        public IActionResult Edit(int id)
        {
            var record = _articleService.GetArticleById(id);
            return View(record);
        }


    }
}
