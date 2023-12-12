using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _23._1News.Areas.Identity.Pages.Account.Manage
{
    public class ManageCategoriesModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public ManageCategoriesModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [BindProperty]
        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }

        public void OnGet()
        {
            Categories = _categoryService.GetAllCategories();
        }

        public IActionResult OnPost()
        {
            var selectedCategory = _categoryService.GetCategotyById(CategoryId);
            return RedirectToAction("ManageCategories");
        }
    }
}
