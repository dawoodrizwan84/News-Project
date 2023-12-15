using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Identity; // Update this namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public Category category { get; set; }





        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found");
            }

            Categories = _categoryService.GetAllCategories() ?? new List<Category>();

            // Load UserCategories explicitly
            user = await _userManager.Users
                .Include(u => u.UserCategories)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            // Ensure UserCategories is not null before accessing it
            user!.UserCategories = user.UserCategories ?? new List<Category>();

            //var selectedCategoryIds = _applicationDbContext.Categories
            //            .Where(cu => cu.CategoryUsers == user.UserCategories)
            //            .Select(cu => cu.Name)
            //            .ToList();


            // Now, you can access all categories selected by the user

            
            var choosenCategories = user.UserCategories.ToList();

            return Page();
        }



        public async Task<IActionResult> OnPost()
        {
            if (SelectCategoryId == 0)
            {
                // Handle the if no category is selected
                ModelState.AddModelError(string.Empty, "Please select a category.");
                return await OnGet();
            }


            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {

                return NotFound("User not found");
            }

            Categories = _categoryService.GetAllCategories() ?? new List<Category>();

            var selectedCategory = Categories?.FirstOrDefault(c => c.CategoryId == SelectCategoryId);


            if (selectedCategory != null)
            {
                user.UserCategories = user.UserCategories ?? new List<Category>();

                if (!user.UserCategories.Any(c => c.CategoryId == selectedCategory.CategoryId))
                {
                    user.UserCategories.Add(selectedCategory);
                }

                //user.UserCategories = user.UserCategories;
                //user.UserCategories.Add(selectedCategory);

                user.ReceiveNewsletters = true;

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




            return RedirectToPage("SuccessPage");
        }
    }
}
