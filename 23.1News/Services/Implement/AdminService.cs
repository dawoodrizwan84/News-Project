using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.ViewModels;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

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
            return _db.Users.ToList();
        }

        public bool DeleteUser(string userId)
        {
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

        public List<IdentityUserRole<string>> GetUserRoles()
        {
           
            return _db.UserRoles.ToList();

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
