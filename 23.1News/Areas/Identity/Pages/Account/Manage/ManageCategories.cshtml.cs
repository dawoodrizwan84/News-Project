using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Identity; // Update this namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _23._1News.Areas.Identity.Pages.Account.Manage
{
    public class ManageCategoriesModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;

        public ManageCategoriesModel(ICategoryService categoryService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _userManager = userManager;
        }

        [BindProperty]
        public int SelectCategoryId { get; set; }

        public List<Category> Categories { get; set; }

     

        public async Task<IActionResult> OnGet()
        {
            Categories = _categoryService.GetAllCategories() ?? new List<Category>();
            return Page();
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            if (SelectCategoryId == 0)
            {
                // Handle the if no category is selected
                ModelState.AddModelError(string.Empty, "Please select a category.");
                return await OnGet();
            }
        

            var user = await _userManager.GetUserAsync(User);

            //if (user != null)
            //{
            //    HttpContext.Session.SetInt32("SelectedCategoryId", user.SelectedCategoryId);
            //}

            if (user == null)
            {
                // Handle the case where the user is not found
                return NotFound("User not found");
            }
            Categories = _categoryService.GetAllCategories() ?? new List<Category>();

            var selectedCategory = Categories.FirstOrDefault(c => c.CategoryId == SelectCategoryId);

            if (selectedCategory != null)
            {
                user.SelectedCategoryId = SelectCategoryId;
                await _userManager.UpdateAsync(user);

                //HttpContext.Session.SetInt32("SelectedCategoryId", SelectCategoryId);
                HttpContext.Session.SetString("SelectedCategoryName", selectedCategory.Name);
            }
            else
            {
                // Handle the case where the selected category is not found
                ModelState.AddModelError(string.Empty, "Selected category not found.");
                return await OnGet();
            }


            //var selectedCategory = _categoryService.GetCategotyById(SelectCategoryId);

            //user.SelectedCategoryId = SelectCategoryId;
            //await _userManager.UpdateAsync(user);

            //HttpContext.Session.SetInt32("SelectedCategoryId", SelectCategoryId);
            //HttpContext.Session.SetString("SelectedCategoryName", Categories.FirstOrDefault(c => c.CategoryId == SelectCategoryId)?.Name);


            //TempData["SelectedCategoryName"] = selectedCategory?.Name;
            //TempData.Keep("SelectedCategoryName");


            // Redirect to a success page or refresh the current page
            return RedirectToPage("SuccessPage");
        }
    }
}
