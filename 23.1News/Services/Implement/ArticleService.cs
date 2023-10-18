using _23._1News.Data;
using _23._1News.Models;
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

        public List<Article> GetArticles()
        {
            return _db.Articles.ToList();
        }

        public void CreateArticle(Article newArticle) 
        {
            _db.Articles.Add(newArticle);
            _db.SaveChanges();
        }

        public bool UpdateArticle(Article newArticle) 
        {
            try
            {
                _db.Articles.Update(newArticle);
                _db.SaveChanges();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public Article GetArticleById(int id) 
        {
            return _db.Articles.Find(id);
        }


        public bool DeleteArticle(int id) 
        {
            try
            {
                var data = this.GetArticleById(id);
                if (data == null)
                {
                    return false;
                }
                _db.Articles.Remove(data);
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
