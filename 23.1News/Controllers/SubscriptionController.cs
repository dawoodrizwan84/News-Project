using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;

namespace _23._1News.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ApplicationDbContext context, ISubscriptionService subscriptionService)
        {
            _context = context;
            _subscriptionService = subscriptionService;
        }

        public IActionResult Index()
        {
            var subsList = _subscriptionService.GetAllSubscription();
            return View(subsList);
        }



        //public IActionResult List()
        //{
           
        //    return View(subscriptionList);
        //}

        [Route("sub")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("sub")]
        public IActionResult Create(Subscription newSubscription)
        {
            _subscriptionService.CreateSubs(newSubscription);
            return RedirectToAction("Index");
        }
    }
}



