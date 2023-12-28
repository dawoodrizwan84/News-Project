using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.View_Models;
using _23._1News.Services.Abstract;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Azure;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace _23._1News.Services.Implement
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public ArticleService(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public List<Article> GetArticles()
        {
            var articles = _db.Articles.Include(a => a.Category)
                .OrderByDescending(a => a.DateStamp)
                .ToList();


            foreach (var item in articles)
            {
                item.BlobLink = GetBlobImage(item.ImageLink);
            }

            return articles;
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
                CategoryId = articleVM.CategoryId,
                Category = _db.Categories
                                .FirstOrDefault(c => c.CategoryId == articleVM.ChosenCategory)!,
                DateStamp = articleVM.DateStamp,
                ImageLink = articleVM.ImageLink,
                EdChoice = articleVM.EdChoice
            };

            dbArt.Author = _db.Users.Find(userId);
            _db.Articles.Add(dbArt);
            _db.SaveChanges();
        }

        public bool UpdateArticle(ArticleVM articleVM)
        {
            Article dbArt = new Article()

            {
                Id = articleVM.Id,
                DateStamp = articleVM.DateStamp,
                LinkText = articleVM.LinkText,
                Headline = articleVM.Headline,
                ContentSummary = articleVM.ContentSummary,
                Content = articleVM.Content,
                CategoryId = articleVM.CategoryId,
                ImageLink = articleVM.ImageLink,
                EdChoice = articleVM.EdChoice
            };

            try
            {
                _db.Update(dbArt);
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
            var article = _db.Articles.Find(id);

            if (article != null)
            {
                article.BlobLink = GetBlobImage(article.ImageLink);
            }
            article!.BlobLink = GetBlobImage(article.ImageLink);
            return article;
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

            foreach (var item in Articles)
            {
                item.BlobLink = GetBlobImage(item.ImageLink);
            }


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

        public async Task<IEnumerable<Article>> GetLatestArticles(int count)
        {

            var latest = await _db.Articles.Include(a => a.Category)
                            .OrderByDescending(a => a.DateStamp)
                            .Take(count).ToListAsync();


            foreach (var article in latest)
            {
                article.BlobLink = GetBlobImage(article.ImageLink);
            }



            return latest;
        }


        public async Task<IEnumerable<Article>> GetEditorsChoice(int count)
        {


            return await _db.Articles.Where(Article => Article.EdChoice == true)
                   .OrderByDescending(a => a.DateStamp).Take(count).ToListAsync();
        }



        private Uri GetBlobImage(string imgLink)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                _configuration["AzureWebJobsStorage"]);
            var blobClient = blobServiceClient.GetBlobContainerClient("newsimages");
            var address = blobClient.GetBlobClient(imgLink).Uri;
            return address;
        }
        private Uri GetSmallBlobImage(string imgLink)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                _configuration["AzureWebJobsStorage"]);
            var blobClient = blobServiceClient.GetBlobContainerClient("resizeimages-sm");
            var address = blobClient.GetBlobClient(imgLink).Uri;
            return address;
        }

        public string UploadImageFile(ArticleVM articleVM)
        {
            IFormFile file = articleVM.File;
            string uniqueFileName = articleVM.ImageLink;
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                _configuration["AzureWebJobsStorage"]);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient("newsimages");
            BlobClient blobClient = blobContainerClient.GetBlobClient(uniqueFileName);

            using (var stream = file.OpenReadStream())
            {
                blobClient.Upload(stream);
            }

            return blobClient.Uri.AbsoluteUri;
        }


        // Search for category articles
        public List<Article> GetArticles(int id)
        {
            var articles = _db.Articles.Where(Article => Article.CategoryId == id)

                            .OrderByDescending(a => a.DateStamp).ToList();


            foreach (var item in articles)
            {
                item.BlobLink = GetBlobImage(item.ImageLink);
            }

            return articles;
        }


        //public List<Article> GetArchiveNews()
        //{
        //    var archiveNews = _db.Articles.Where(a => a.DateStamp.Date == DateTime.Today
        //                                .AddDays(-30)).ToList();

        //    foreach (var item in archiveNews)
        //    {
        //        item.Archived = true;
        //    }


        //    _db.SaveChanges();
        //    return archiveNews;
        //}


        [Authorize("Admin")]
        public List<Article> GetArchiveNews()
        {
            var thirtyDaysAgo = DateTime.Today.AddDays(-30);

            var archiveNews = _db.Articles
             .Where(a => a.DateStamp.Date <= thirtyDaysAgo && !a.Archived)
             .ToList();

            foreach (var item in archiveNews)
            {
                item.Archived = true;
            }


            _db.SaveChanges();
            return archiveNews;
        }


        [Authorize("Admin")]

        public List<Article> SearchArhivedNews(string searchTerm)
        {
            DateTime? datestamp = null;
            string datePattern = @"^\d{4}-\d{2}-\d{2}$";

            if (Regex.IsMatch(searchTerm, datePattern))
            {
                datestamp = DateTime.Parse(searchTerm).Date;
            }


            var thirtyDaysAgo = DateTime.Today.AddDays(-30);

            var archivedArticles = _db.Articles
                .Where(article => article.Archived && article.DateStamp.Date <= thirtyDaysAgo)
                .ToList();



            var Articles = _db.Articles.Where(Article => Article.Archived == true).ToList();
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

        //public List<Article> SearchArchivedNews(string searchTerm)
        //{
        //    DateTime? datestamp = null;
        //    string datePattern = @"^\d{4}-\d{2}-\d{2}$";

        //    if (Regex.IsMatch(searchTerm, datePattern))
        //    {
        //        datestamp = DateTime.Parse(searchTerm).Date;
        //    }

        //    var thirtyDaysAgo = DateTime.Today.AddDays(-30);

        //    var archivedArticles = _db.Articles
        //        .Where(article => article.Archived && article.DateStamp.Date <= thirtyDaysAgo)
        //        .ToList();

        //    var searchResults = archivedArticles
        //        .Where(article =>
        //            article.Headline.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //            article.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //            article.ContentSummary.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //            article.LinkText.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
        //            (datestamp != null && article.DateStamp.Date == datestamp)
        //        )
        //        .ToList();

        //    return searchResults;
        //}

        public List<Article> GetFirstArticleInCategory()
        {

            var articles = _db.Articles.GroupBy(x => x.CategoryId)
            .Select(g => g.OrderByDescending(x => x.DateStamp).First()).ToList();


            foreach (var item in articles)
            {
                item.BlobLink = GetBlobImage(item.ImageLink);
            }

            return articles;
        }


    }
}
