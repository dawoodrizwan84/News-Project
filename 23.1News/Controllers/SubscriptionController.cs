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
using _23._1News.Services.Implement;
using _23._1News.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using _23._1News.Models.Email;
using _23._1News.Helpers;

namespace _23._1News.Controllers
{
    public class SubscriptionController : Controller
    {

        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger<SubscriptionController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmailHelper _emailHelper;

        public SubscriptionController(ApplicationDbContext applicationDbContext,
            ISubscriptionService subscriptionService,
            ILogger<SubscriptionController> logger,
            IEmailHelper emailHelper)
        {

            _subscriptionService = subscriptionService;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _emailHelper = emailHelper;
        }


        public IActionResult Index()
        {
            var subsList = _subscriptionService.GetAllSubs();
            return View(subsList);
        }

        public IActionResult Info()
        {
            return View();
        }


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

        public IActionResult Delete(int id)
        {
            _subscriptionService.DeleteSubs(id);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var record = _subscriptionService.GetSubsById(id);
            return View(record);
        }


        [HttpPost]
        public IActionResult Edit(Subscription newSubs)
        {
            if (!ModelState.IsValid)
            {
                return View(newSubs);
            }
            var result = _subscriptionService.UpdateSubs(newSubs);
            if (result)
            {
                return RedirectToAction("Index");
            }

            return View(newSubs);
        }

        public IActionResult Details(int id)
        {
            var det = _subscriptionService.GetSubsById(id);

            if (det == null)
            {
                return NotFound();
            }
            return View(det);
        }

        public IActionResult SendEmail()
        {
            EmailMessage newEmail = new EmailMessage()
            {
                FromAddress = new EmailAddress()
                {
                    Address = "senderemailservice23.1@gmail.com",
                    Name = "23.1News"
                },
                Content = "Test to see if this works",
                Subject = "Test"
            };

            newEmail.ToAddresses.Add(new EmailAddress()
            {
                Address = "xinliu1108@gmail.com",
                Name = "Xin"
            });
            _emailHelper.SendEmail(newEmail);
            return View();
        }
    }
}



