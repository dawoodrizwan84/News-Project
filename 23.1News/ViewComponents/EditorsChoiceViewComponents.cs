using _23._1News.Services.Abstract;
using _23._1News.Services.Implement;
using _23._1News.Services;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.ViewComponents
{
    public class EditorsChoiceViewComponent : ViewComponent
    {

        private readonly IArticleService _articleService;


        public EditorsChoiceViewComponent(IArticleService articleService)
        {
            _articleService = articleService;

        }

        public async Task<IViewComponentResult> InvokeAsync(string count)
        {
            var intCount = Convert.ToInt32(count);
            var editorsChoice = await _articleService.GetEditorsChoice(intCount);
            return View("Default", editorsChoice);
        }

    }
}