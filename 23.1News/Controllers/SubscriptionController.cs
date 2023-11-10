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
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace _23._1News.Controllers
{
    public class SubscriptionController : Controller
    {

        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger<SubscriptionController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmailHelper _emailHelper;
        private readonly UserManager<User> _userManager;

        public SubscriptionController(ApplicationDbContext applicationDbContext,
            ISubscriptionService subscriptionService,
            ILogger<SubscriptionController> logger,
            UserManager<User> userManager,
            IEmailHelper emailHelper)
        {

            _subscriptionService = subscriptionService;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _emailHelper = emailHelper;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var subsList = _subscriptionService.GetAllSubs();
            return View(subsList);
        }



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Subscription newSubscription)
        {
            newSubscription.User = _userManager.GetUserAsync(User).Result;
            _subscriptionService.CreateSubs(newSubscription);
            SendEmail(newSubscription);
            return RedirectToAction("Index","Home");
        }


        //[HttpPost]
        //[Route("sub")]
        //public async Task<IActionResult> Create(Subscription newSubscription)
        //{
        //    var user = new User
        //    {
        //        UserName = newSubscription.UserName,
        //        Email = newSubscription.Email
        //    };


        //    var result = await _userManager.CreateAsync(user);

        //    if (result.Succeeded)
        //    {
        //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);

        //        EmailMessage newEmail = new EmailMessage()
        //        {
        //            FromAddress = new EmailAddress()
        //            {
        //                Address = "senderemailservice23.1@gmail.com",
        //                Name = "23.1News"
        //            },
        //            Content = "Thank you for subscribing!.",
        //            Subject = "Welcome to 23.1 News"
        //        };

        //        newEmail.ToAddresses.Add(new EmailAddress()
        //        {
        //            Address = newSubscription.UserName,
        //            Name = newSubscription.Email
        //        });

        //        _emailHelper.SendEmail(newEmail);

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Error");
        //    }
        //}

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


        public IActionResult SendEmail(Subscription newSubscription)
        {

            EmailMessage newEmail = new EmailMessage()
            {
                FromAddress = new EmailAddress()
                {
                    Address = "senderemailservice23.1@gmail.com",
                    Name = "23.1News"
                },
                Content = "Thank you for subscribing!",
                Subject = "Welcome to 23.1 News"
            };

            newEmail.ToAddresses.Add(new EmailAddress()
            {
                Address = newSubscription.User.Email,
                Name = newSubscription.User.FirstName + " " + newSubscription.User.LastName
            });

            _emailHelper.SendEmail(newEmail);

            return View();
        }


    }
}




