using _23._1News.Models.Db;

namespace _23._1News.Services.Abstract
{
    public interface IArticleService
    {
        List<Article> GetArticles();

        void CreateArticle(Article article);
        bool UpdateArticle(Article article);

        Article GetArticleById(int id);
        bool DeleteArticle(int id);


    }
}
