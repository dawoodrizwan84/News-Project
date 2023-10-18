using _23._1News.Data;
using _23._1News.Services.Abstract;

namespace _23._1News.Services.Implement
{
    public class ArticleService : IArticleService 
    {
        private readonly ApplicationDbContext _db;

        public ArticleService(ApplicationDbContext db) 
        {
            _db = db;
        }

    }
}
