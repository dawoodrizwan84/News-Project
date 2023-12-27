using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _23._1News.Controllers
{
    public class ExchangeController : Controller
    {

        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly ISubscriptionService _subscriptionService;

        public ExchangeController(IExchangeRatesService exchangeRatesService, ISubscriptionService subscriptionService)
        {
            _exchangeRatesService = exchangeRatesService;
            _subscriptionService = subscriptionService;
        }

       
     
        public async Task<IActionResult> LatestRates()
        {
            // Get the user ID of the logged-in user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null && _subscriptionService.isEnteprise(userId))
            {
                var newRates = await _exchangeRatesService.GetRateAsync();
                return View(newRates);
            }

            return RedirectToAction("Index", "Home");
        }


        [Route("nr")]
        public async Task<IActionResult> NewRates() 
        {
            var newR = await _exchangeRatesService.GetRateAsync(); 
            var filter = newR.Rates.Take(10).Distinct().ToList();
           
            return View(filter);
        }

    }
}
