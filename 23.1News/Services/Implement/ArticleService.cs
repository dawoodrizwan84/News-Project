using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.View_Models;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.RegularExpressions;

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

        public void CreateArticle(ArticleVM articleVM, string userId)
        {
           //_db.Add(articleVM);

            Article dbArt = new Article()
            {
                Headline = articleVM.Headline,
                Content = articleVM.Content,
                ContentSummary = articleVM.ContentSummary,
                LinkText = articleVM.LinkText,
                //Category = _db.Categories
                        //.Where(c => c.Name == articleVM.ChosenCategory)
                        //.FirstOrDefault()!,
                DateStamp = articleVM.DateStamp,
                ImageLink = articleVM.ImageLink,
            };
            
            //dbArt.Author = _db.Users.Find(userId);
            _db.Articles.Add(dbArt);
            _db.SaveChanges();
                     


        }

        public bool UpdateArticle(ArticleVM articleVM) 
        {
            try
            {
                _db.Update(articleVM);
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
        public List<Article> SearchArticle(string searchTerm)
        {
            DateTime? datestamp = null;
            string datePattern = @"^\d{4}-\d{2}-\d{2}$";

            if (Regex.IsMatch(searchTerm, datePattern))
            {
                datestamp = DateTime.Parse(searchTerm).Date;
            }

            var Articles = _db.Articles.ToList();
            var searchResults = Articles
                .Where(article =>
                    article.Headline.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    article.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    article.ContentSummary.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    article.LinkText.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (datestamp != null && article.DateStamp.Date == datestamp)
                )
                .ToList();

            return searchResults;
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

        public List<Category> GetCategories() 
        {
           return _db.Categories.ToList();
        }

    }
}
