using _23._1News.Models.Db;
using _23._1News.Models.View_Models;

namespace _23._1News.Services.Abstract
{
    public interface IArticleService
    {
        List<Article> GetArticles();
        List<Article> GetArticles(int id);
        List<Article> SearchArticle(string Headline);
        void CreateArticle(ArticleVM articleVM, string userId);

        Task<IEnumerable<Article>> GetLatestArticles(int count);

        Task<IEnumerable<Article>> GetEditorsChoice(int count);

        bool UpdateArticle(ArticleVM articleVM);

        Article GetArticleById(int id);
        bool DeleteArticle(int id);

        List<Category> GetCategories();

        string UploadImageFile(ArticleVM articleVM);

        List<Article> GetArchiveNews();
        List<Article> SearchArhivedNews(string Headline);

        public List<Article> GetFirstArticleInCategory();

    }
}
