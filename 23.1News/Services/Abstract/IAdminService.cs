using _23._1News.Models.Db;
using _23._1News.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace _23._1News.Services.Abstract
{
    public interface IAdminService
    {
        List<Article> GetAllArticles();

        List<User> GetAllUsers();

        bool DeleteUser(string userId);

        List<IdentityRole> GetAllRoles();

        List<IdentityUserRole<string>> GetUserRoles();

        
        bool AddUserRole(UserRoleVM userRoleVM);
       
    }
}
