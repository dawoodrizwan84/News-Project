using _23._1News.Services.Abstract;
using _23._1News.Services.Implement;
using _23._1News.Services;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.ViewComponents
{
    public class LatestNewsViewComponent : ViewComponent
    {

        private readonly IArticleService _articleService;


        public LatestNewsViewComponent(IArticleService articleService)
        {
            _articleService = articleService;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var latestNews = _articleService.GetArticles();
            var latestNews = await _articleService.GetLatestArticles(10);
            return View("Default", latestNews);
        }

    }
}
