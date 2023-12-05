using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger,
            ApplicationDbContext applicationDbContext, IEmployeeService employeeService)
        {
            _applicationDbContext = applicationDbContext;
            _employeeService = employeeService;
            _logger = logger;
        }

        [Route("empl")]
        public IActionResult EmployeeList()
        {
            var empList = _employeeService.GetEmployees();
            return View(empList);
        }

        
        [HttpGet]
        public IActionResult CreateEmployee()
        {
            return View();
        }

      
        [HttpPost]
        public IActionResult CreateEmployee(Employee employee)
        {
            _employeeService.AddEmployee(employee);

            return RedirectToAction("EmployeeList");
        }
    }
}
