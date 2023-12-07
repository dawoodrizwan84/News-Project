using _23._1News.Models.Db;
using _23._1News.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace _23._1News.Services.Abstract
{
    public interface IEmployeeService
    {
        List<User> GetEmployees();

        void AddEmployee(EmployeeVM employeeVM);

        //Task<string> GetEmployeeRoleAsync(int employeeId);
        //Task<bool> AssignRoleAsync(int employeeId, string roleName);

        //Employee GetEmployeeById(int employeeId);

        //bool DeleteEmployee(int employeeId);

    }
}
