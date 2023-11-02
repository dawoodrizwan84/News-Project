using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ICategoryService _categoryService;
        public CategoryController(ApplicationDbContext applicationDbContext, 
            ICategoryService categoryService) 
        {
            _applicationDbContext = applicationDbContext;
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            var categoryList = _applicationDbContext.Categories.ToList();
            return View(categoryList);
        }

      
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
      
        public IActionResult Create(Category category)
        {
            if (ModelState. IsValid) 
            {
                _categoryService.AddCategory(category);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) 
        {
            _categoryService.DeleteCategoty(id);
            return RedirectToAction("Index");
        }
    }
}
