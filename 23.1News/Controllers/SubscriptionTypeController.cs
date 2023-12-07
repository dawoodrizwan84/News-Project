using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.View_Models;
using _23._1News.Services.Abstract;
using _23._1News.Services.Implement;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubscriptionType subscriptionType)
        {
            if (ModelState.IsValid)
            {
                _subscriptionTypeService.AddSubscriptionType(subscriptionType);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var subTypeId = _subscriptionTypeService.GetSubscriptionTypeById(id);

            SubscriptionType subscriptionType = new SubscriptionType();
            subscriptionType.Id = subTypeId.Id;
            subscriptionType.TypeName = subTypeId.TypeName;
            subscriptionType.Price = subTypeId.Price;
            subscriptionType.Description = subTypeId.Description;

            return View("Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SubscriptionType subscriptionType)
        {

            var result = _subscriptionTypeService.UpdateSubscriptionType(subscriptionType);
            if (result)
            {
                return RedirectToAction("Index");
            }

            return View("Edit");
        }


        public IActionResult Details(int id)
        {
            var det = _subscriptionTypeService.GetSubscriptionTypeById(id);

            if (det == null)
            {
                return NotFound();
            }
            return View(det);
        }
        public IActionResult Delete(int id)
        {
            _subscriptionTypeService.DeleteSubscriptionType(id);
            return RedirectToAction("Index");
        }

        [Authorize]

        public IActionResult GotoStart(int id)
        {
            SubscriptionType subscriptionType = _subscriptionTypeService.GetSubscriptionTypeById(id);
            TempData["subTypeId"] = subscriptionType.Id;
            TempData["subTypeName"] = subscriptionType.TypeName;
            TempData["subTypePrice"] = subscriptionType.Price;
            TempData["subTypeDescription"] = subscriptionType.Description;
            return RedirectToAction("Create", "Subscription");
        }
    }
}


