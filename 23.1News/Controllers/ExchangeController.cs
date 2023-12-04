using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{
    public class ExchangeController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public ExchangeController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;

        }



        public async Task<IActionResult> Index()
        {
            var newRate = await _currencyService.GetRate();

            return View(newRate);


        }

    }
}
