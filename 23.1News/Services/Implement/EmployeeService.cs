using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace _23._1News.Services.Implement
{
    public class EmployeeService : IEmployeeService
    {
        private ApplicationDbContext? _db;
        private readonly UserManager<User>? _userManager;
        private readonly RoleManager<IdentityRole>? _roleManager;


        public EmployeeService(RoleManager<IdentityRole>? roleManager, ApplicationDbContext db,
                               UserManager<User> userManager)
        {

            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;

        }

        public List<Employee> GetEmployees()
        {
            return _db.Employees.ToList();
        }

        public void AddEmployee(Employee employee)
        {
            _db?.Employees.Add(employee);
            _db?.SaveChanges();
        }

        public Employee GetEmployeeById(int employeeId)
        {
            var details = _db.Employees.Find(employeeId);
            return details;
        }

        public bool DeleteEmployee(int employeeId)
        {
            try
            {
                var data = this.GetEmployeeById(employeeId);
                if (data == null)
                {
                    return false;
                }
                _db.Employees.Remove(data);
                _db.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
