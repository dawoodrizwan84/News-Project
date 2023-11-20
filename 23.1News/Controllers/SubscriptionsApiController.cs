using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{

    [ApiController]
    [Route("api/subscriptions")]
    public class SubscriptionsApiController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;

        public IActionResult Index()
        {
            return View();
        }


        public SubscriptionsApiController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("count")]
        public IActionResult GetSubscriptionCount()
        {
            var count = _subscriptionService.GetActiveSubscribersCount();
            return Ok(new { Count = count });
        }
    }
}
