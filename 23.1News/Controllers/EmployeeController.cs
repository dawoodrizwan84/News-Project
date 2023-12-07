using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.ViewModels;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _23._1News.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public EmployeeController(ILogger<EmployeeController> logger,
            ApplicationDbContext applicationDbContext, IEmployeeService employeeService,
            RoleManager<IdentityRole> roleManager)
        {
            _applicationDbContext = applicationDbContext;
            _employeeService = employeeService;
            _roleManager = roleManager;
            _logger = logger;
        }


        public IActionResult EmployeeList()
        {
           var employeeList = _employeeService.GetEmployees();
            return View(employeeList);
           
        }


        [HttpGet]
       
        public IActionResult CreateEmployee()
        {
            // Retrieve roles for dropdown list
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToList();

            return View();
        }


        [HttpPost]
       
        public IActionResult CreateEmployee(EmployeeVM employeeVM)
        {


            //var roleEntity = _roleManager.FindByNameAsync(employeeVM).Result;

            _employeeService.AddEmployee(employeeVM);

            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

           return RedirectToAction("EmployeeList");
        }




        //public async Task<IActionResult> AssignRole(int employeeId, string roleName)
        //{
        //    var success = _employeeService.AssignRoleAsync(employeeId, roleName);
        //    return View(success);
        //}
    }
}
