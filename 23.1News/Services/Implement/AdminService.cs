using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;

namespace _23._1News.Services.Implement
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _db;

        public AdminService(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<Article> GetAllArticles() 
        {
            return _db.Articles.ToList();
        }
    }
}
