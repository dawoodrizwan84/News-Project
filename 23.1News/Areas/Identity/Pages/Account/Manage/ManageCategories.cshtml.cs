using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Identity; // Update this namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _23._1News.Areas.Identity.Pages.Account.Manage
{
    public class ManageCategoriesModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;


        public ManageCategoriesModel(ICategoryService categoryService, UserManager<User> userManager,
                    ApplicationDbContext applicationDbContext)
        {
            _categoryService = categoryService;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public int SelectCategoryId { get; set; }


        public List<Category> Categories { get; set; }

        //public Category category { get; set; }





        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found");
            }

            Categories = _categoryService.GetAllCategories() ?? new List<Category>();

            user = await _userManager.Users
                .Include(u => u.UserCategories)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            user!.UserCategories = user.UserCategories ?? new List<Category>();

            var choosenCate = user.UserCategories.ToList();
            //TempData["success"] = Categories;


            return Page();

        }



        public async Task<IActionResult> OnPost()
        {
            if (SelectCategoryId == 0)
            {
                // Handle the case if no category is selected
                ModelState.AddModelError(string.Empty, "Please select a category.");
                return await OnGet();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var Categories = _categoryService.GetAllCategories() ?? new List<Category>();
            var selectedCategory = Categories?.FirstOrDefault(c => c.CategoryId == SelectCategoryId);

            if (selectedCategory != null)
            {
                user.UserCategories = user.UserCategories ?? new List<Category>();

                // Check if the user already has the selected category
                user.UserCategories.Add(selectedCategory);

                if (user.UserCategories.Any(c => c.CategoryId == selectedCategory.CategoryId))
                {
                    ModelState.AddModelError(string.Empty, "Selected category already chosen.");
                    return await OnGet();
                }

                user.ReceiveNewsletters = true;

                // Update the user
                //await _userManager.UpdateAsync(user);

                HttpContext.Session.SetString("SelectedCategoryName", selectedCategory.Name);
            }
            else
            {
                // Handle the case where the selected category is not found
                ModelState.AddModelError(string.Empty, "Selected category not found.");
                return await OnGet();
            }

            // Handle successful case or redirect as needed
            return RedirectToPage("/SuccessPage");
        }

    }
}
