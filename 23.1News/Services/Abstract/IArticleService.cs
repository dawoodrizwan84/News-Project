using _23._1News.Models.Db;
using _23._1News.Models.View_Models;

namespace _23._1News.Services.Abstract
{
    public interface IArticleService
    {
        List<Article> GetArticles();
        List<Article> SearchArticle(string Headline);
        void CreateArticle(ArticleVM articleVM, string userId);
        bool UpdateArticle(ArticleVM articleVM);

        Article GetArticleById(int id);
        bool DeleteArticle(int id);

        List<Category> GetCategories();

        void UploadImageFile(IFormFile file);

    }
}
