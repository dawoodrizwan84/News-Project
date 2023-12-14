using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.ViewModels;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;






namespace _23._1News.Services.Implement
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;


        public AdminService(ApplicationDbContext db, RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;

        }
        public List<Article> GetAllArticles()
        {

            return _db.Articles.ToList();
        }

        public List<User> GetAllUsers()
        {
            var userCate = _db.Users.ToList();
            return userCate;
        }

        //public User GetUserById(string id)
        //{
        //   User user = _db.Users
        //               .Single(p => p.Id.Equals(id));

        //    return user;
        //}




        public bool DeleteUser(string userId)
        {
            var userSubscriptions = _db.Subscriptions.Where(s => s.UserId == userId).ToList();

            // delete subscription
            _db.Subscriptions.RemoveRange(userSubscriptions);
            _db.SaveChanges();

            var user = _db.Users.Find(userId);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                return true;
            }

            return false;
        }


        public List<IdentityRole> GetAllRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<bool> AddRole(string roleName)
        {
            // Check if the role already exists
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                // Role already exists, return false or handle accordingly
                return false;
            }

            // Create a new IdentityRole instance
            var newRole = new IdentityRole(roleName);

            // Create the role using the RoleManager
            var result = await _roleManager.CreateAsync(newRole);

            // Check if the role creation was successful

            return result.Succeeded;
        }


        //public List<IdentityUserRole<string>> GetUserRoles()
        //{

        //    return _db.UserRoles.ToList();

        //}

        public List<UserRoleVM> GetUserRoles()
        {
            var userRoleDetails = _db.UserRoles
                .Join(
                    _db.Users,
                    ur => ur.UserId,
                    u => u.Id,
                    (ur, u) => new
                    {
                        ur.RoleId,
                        ur.UserId,
                        u.UserName, // Include user name
                        u.FirstName, // Assuming FirstName and LastName are properties in your User model
                        u.LastName

                    })
                .Join(_roleManager.Roles,
                     ur => ur.RoleId,
                     r => r.Id,
                     (ur, r) => new
                     {
                         ur.RoleId,
                         ur.UserId,
                         ur.FirstName,
                         ur.LastName,
                         RoleName = r.Name
                     }).ToList();

            var userRolesViewModel = userRoleDetails.Select(ur => new UserRoleVM
            {
                UserId = ur.UserId,
                UserName = $"{ur.FirstName} {ur.LastName}", // Concatenate first and last name
                RoleId = ur.RoleId,
                RoleName = ur.RoleName

                // Include other properties from UserRoles, if needed
            }).ToList();

            return userRolesViewModel;
        }



        public bool AddUserRole(UserRoleVM userRoleVM)
        {
            try
            {
                var user = _db.Users.Where(u => u.Id == userRoleVM.UserId)
                        .FirstOrDefault();
                var role = _roleManager.FindByIdAsync(userRoleVM.RoleId).Result;
                if (user != null && role != null)
                {
                    var result = _userManager.AddToRoleAsync(user, role.Name).Result;
                    return result.Succeeded;
                }

                return false;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
