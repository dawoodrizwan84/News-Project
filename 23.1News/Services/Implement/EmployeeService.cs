using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.ViewModels;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _23._1News.Services.Implement
{
    public class EmployeeService : IEmployeeService
    {
        private ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public EmployeeService(RoleManager<IdentityRole> roleManager, ApplicationDbContext db,
                               UserManager<User> userManager)
        {

            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;

        }

        //public async Task<string> GetEmployeeRoleAsync(int employeeId)
        //{
        //    var roleName = await _db.Employees
        //        .Where(e => e.Id == employeeId)
        //        .Select(e => e.IdentityRole.Name)
        //        .FirstOrDefaultAsync();

        //    return roleName;
        //}

        //public async Task<bool> AssignRoleAsync(int employeeId, string roleName)
        //{
        //    var employee = await _db.Employees.FindAsync(employeeId);

        //    if (employee != null)
        //    {
        //        // Fetch the role entity based on the role name
        //        var roleEntity = await _roleManager.FindByNameAsync(roleName);

        //        if (roleEntity != null)
        //        {
        //            // Set the navigation property to the new role entity
        //            employee.IdentityRole = roleEntity;
        //            await _db.SaveChangesAsync();
        //            return true;
        //        }
        //    }

        //    return false;
        //}


        public List<User> GetEmployees()
        {
            var editors = _userManager.GetUsersInRoleAsync("Editor").Result.ToList();
            var admins = _userManager.GetUsersInRoleAsync("Admin").Result.ToList();
            var combined = new List<User>();
            combined.AddRange(editors);
            combined.AddRange(admins);
            return combined.Distinct().ToList();
            //return _db.Users.ToList();
        }

        public void AddEmployee(EmployeeVM employeeVM)
        {

            //var userEntity = _userManager.FindByIdAsync(userId);
            
            //if (userEntity != null) 
            //{
            var dbEmp = new User()
            {
                FirstName = employeeVM.FirstName,
                LastName = employeeVM.LastName,
                //Role = employeeVM.Role,
                Address = employeeVM.Address,
                Email = employeeVM.Email,
                EmailConfirmed = true,
                UserName = employeeVM.Email,
                PhoneNumber = employeeVM.PhoneNumber,
                                 
                //DOB = employeeVM.DateOfBirth,


                //User = userEntity,
                //IdentityRole = roleId,

                    
            };

            var result = _userManager.CreateAsync(dbEmp, "Password_123").Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(dbEmp, employeeVM.Role).Wait();
                // do something
            }

            // handle error

                //_db?.Employees.Add(dbEmp);
                //_db?.SaveChanges();
            //}



            // Retrieve the user and role entities from the database based on their IDs


            //dbEmp.User = userEntity;
            //dbEmp.IdentityRole = role;




        }

        //public Employee GetEmployeeById(int employeeId)
        //{
        //    var details = _db.Employees.Find(employeeId);
        //    return details;
        //}

        //public bool DeleteEmployee(int employeeId)
        //{
        //    try
        //    {
        //        var data = this.GetEmployeeById(employeeId);
        //        if (data == null)
        //        {
        //            return false;
        //        }
        //        _db.Employees.Remove(data);
        //        _db.SaveChanges();
        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}
