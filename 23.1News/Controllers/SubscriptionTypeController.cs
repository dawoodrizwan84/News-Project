using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.View_Models;
using _23._1News.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{
    public class SubscriptionTypeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ISubscriptionTypeService _subscriptionTypeService;
        public SubscriptionTypeController(ApplicationDbContext applicationDbContext,
            ISubscriptionTypeService subscriptionTypeService)
        {
            _applicationDbContext = applicationDbContext;
            _subscriptionTypeService = subscriptionTypeService;
        }

        public IActionResult Index()
        {
            var subscriptionTypeList = _applicationDbContext.SubscriptionTypes.ToList();
            return View(subscriptionTypeList);
        }
        public IActionResult List()
        {
            var subscriptionTypeList = _applicationDbContext.SubscriptionTypes.ToList();
            return View(subscriptionTypeList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(SubscriptionType subscriptionType)
        {
            if (ModelState.IsValid)
            {
                _subscriptionTypeService.AddSubscriptionType(subscriptionType);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _subscriptionTypeService.DeleteSubscriptionType(id);
            return RedirectToAction("Index");
        }
    }
}


