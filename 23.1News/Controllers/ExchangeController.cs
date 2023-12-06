using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{
    public class ExchangeController : Controller
    {

        private readonly IExchangeRatesService _exchangeRatesService;

        public ExchangeController(IExchangeRatesService exchangeRatesService)
        {
            _exchangeRatesService = exchangeRatesService;
        }

        public async Task<IActionResult> LatestRates()
        {
            var newRates = await _exchangeRatesService.GetRateAsync();
            return View(newRates);
        }
    }
}
