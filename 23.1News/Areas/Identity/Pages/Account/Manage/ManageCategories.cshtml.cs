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



        public ManageCategoriesModel(ICategoryService categoryService,
            UserManager<User> userManager, ApplicationDbContext applicationDbContext)
        {
            _categoryService = categoryService;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [BindProperty]
        public int SelectCategoryId { get; set; }

        [BindProperty]
        public List<Category> Categories { get; set; }

        //public List<User> Users { get; set; }

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

            var que = user.UserCategories
                .Where(wh => wh.Name == wh.Name).ToList();

            var que2 = que;
            TempData["data"] = que2;

            //var choosenCate = user.UserCategories.ToList();
            //var slect = user.UserCategories.Where(user => choosenCate.Any());

            //ViewData["selected"] = select;
            //TempData.Keep("selected");




            return Page();

        }


        public async Task<IActionResult> OnPost()
        {
            if (SelectCategoryId == 0)
            {

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

                user.UserCategories.Add(selectedCategory);

                var existingRelationship = _applicationDbContext.Users
                .Any(u => u.Id == user.Id && u.UserCategories
                .Any(uc => uc.CategoryId == selectedCategory.CategoryId));

                if (existingRelationship)
                {
                    ModelState.AddModelError(string.Empty, "Selected category already chosen.");
                    // Handle the case where the category is already chosen, e.g., return a view or perform other actions.
                    return await OnGet();
                }



                HttpContext.Session.SetString("SelectedCategoryName", selectedCategory.Name);
            }
            else
            {
                // Handle the case where the selected category is not found
                ModelState.AddModelError(string.Empty, "Selected category not found.");
                return await OnGet();
            }


            //user.UserCategories.Remove(selectedCategory);
            await _userManager.UpdateAsync(user);

            // Handle successful case or redirect as needed
            return RedirectToPage(OnGet);
        }

    }







}
