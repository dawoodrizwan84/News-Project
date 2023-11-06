using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.ViewComponents
{
    
    public class LatestNewsViewComponents : ViewComponent
    {
        private readonly IArticleService _articleService;
        public LatestNewsViewComponents(IArticleService articleService)
        {
            _articleService = articleService;
        }
      
    }
}
