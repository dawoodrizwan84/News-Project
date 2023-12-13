using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.ViewModels;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IAdminService _adminService;
        private readonly IArticleService _articleService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;



        public AdminController(ILogger<AdminController> logger,
            IArticleService articleService, IAdminService adminService,
            ApplicationDbContext applicationDbContext,
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _logger = logger;
            _adminService = adminService;
            _applicationDbContext = applicationDbContext;
            _articleService = articleService;
            _roleManager = roleManager;
            _userManager = userManager;


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


        public IActionResult UserList()
        {
            var users = _adminService.GetAllUsers();
            return View(users);
        }

        public IActionResult DelUser(string id)
        {
            var delete = _adminService.DeleteUser(id);

            if (delete)
            {

                return RedirectToAction("UserList");
            }
            else
            {
                return View("ErrorView");
            }
        }

        [Route("rol")]
        public IActionResult RoleList()
        {
            var roles = _adminService.GetAllRoles();
            return View(roles);
        }

        [Route("anr")]
        public IActionResult AddNewRole()
        {


            return View();


        }

        [HttpPost]
        [Route("anr")]
        public async Task<IActionResult> AddNewRole(string roleName)
        {
            var result = await _adminService.AddRole(roleName);

            if (result)
            {
                // Role added successfully, you can redirect to the RoleList action or handle accordingly
                return RedirectToAction("RoleList");
            }
            else
            {
                // Role creation failed, handle accordingly (e.g., return an error view)
                return View("Error");
            }


        }


        public IActionResult UserRoles()
        {
            var userRoles = _adminService.GetUserRoles();
                              
            return View("UserRoles", userRoles);
        }




        public IActionResult CreateRoles()
        {
            UserRoleVM addUserRoleVM = new UserRoleVM();
            return View(addUserRoleVM);
        }

        [HttpPost]

        public IActionResult CreateRoles(UserRoleVM userRoleVM)
        {
            var aaa = _adminService.AddUserRole(userRoleVM);
            if (!aaa)
            {
                TempData["msg"] = "Not succeeded";
                return RedirectToAction("CreateRoles");
            }

            return RedirectToAction("UserRoles");

        }



    }
}
