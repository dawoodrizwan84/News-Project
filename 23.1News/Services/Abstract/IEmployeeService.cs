using _23._1News.Models.Db;

namespace _23._1News.Services.Abstract
{
    public interface IEmployeeService
    {
        List<Employee> GetEmployees();

        void AddEmployee(Employee employee);

        Employee GetEmployeeById(int employeeId);

        bool DeleteEmployee(int employeeId);

    }
}
