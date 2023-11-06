using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace _23._1News.Services.Implement
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
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
    }
}
