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
using _23._1News.Models.ViewModels;
using System.Security.Claims;

namespace _23._1News.Controllers
{
    public class SubscriptionController : Controller
    {

        private readonly ISubscriptionService _subscriptionService;
        private readonly ISubscriptionTypeService _subscriptionTypeService;
        private readonly ILogger<SubscriptionController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmailHelper _emailHelper;
        private readonly UserManager<User> _userManager;

        public SubscriptionController(ApplicationDbContext applicationDbContext,
            ISubscriptionService subscriptionService,
            ISubscriptionTypeService subscriptionTypeService,
            ILogger<SubscriptionController> logger,
            UserManager<User> userManager,
            IEmailHelper emailHelper)
        {

            _subscriptionService = subscriptionService;
            _subscriptionTypeService = subscriptionTypeService;
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
            var subTypeId = TempData["subTypeId"];
            TempData.Keep("subTypeId");
            
            return View();
        }


        //[HttpPost]
        //public IActionResult Create(Subscription newSubscription)
        //{
        //    var subTypeId = (int)TempData["subTypeId"]!;
        //    newSubscription.SubscriptionTypeId = subTypeId;
        //    newSubscription.User = _userManager.GetUserAsync(User).Result;
        //    _subscriptionService.CreateSubs(newSubscription);
        //    SendEmail(newSubscription);
        //    return RedirectToAction("Index",new {id=newSubscription.Id});
        //}


        [HttpPost]
        public IActionResult Create(Subscription newSubscription)
        {
            // Get the current user
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Check if the user is already subscribed
            var existingSubscription = _subscriptionService.GetActiveSubscriptionByUser(currentUser.Id);

            //if (existingSubscription != null)
            //{
            //    // User is already subscribed, you may want to handle this case (e.g., display an error message)
            //    // For now, redirect to the existing subscription details
            //    return RedirectToAction("Details", new { id = existingSubscription.Id });
            //}
          

            // If the user is not already subscribed, proceed with creating a new subscription
            var subTypeId = (int)TempData["subTypeId"]!;
            newSubscription.SubscriptionTypeId = subTypeId;
            newSubscription.User = currentUser;
            _subscriptionService.CreateSubs(newSubscription);
            SendEmail(newSubscription);

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(30));
            var userid = newSubscription.User.Id;
            HttpContext.Response.Cookies.Append("user_id", userid, cookieOptions);

            //return RedirectToAction("Index", new { id = newSubscription.Id });
            return RedirectToAction("Index", "Home", new { id = newSubscription.Id });



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

            var usr = _subscriptionService.GetUserById(det.UserId);


           if (usr == null)
            {
                return NotFound();
            }


            TempData["FirstName"] = usr.FirstName;
            TempData["LastName"] = usr.LastName;
            TempData["Email"] = usr.Email;

 

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
                Subject = "Welcome to 23.1 News"
            };

            // Determine subscription type
            string subscriptionTypeMessage = string.Empty;
            switch (newSubscription.SubscriptionType.TypeName.ToLower())
            {
                case "free":
                    subscriptionTypeMessage = "Welcome to the Free Subscription!";
                    break;
                case "pro":
                    subscriptionTypeMessage = "Welcome to the Pro Subscription! Thank you for upgrading.";
                    break;
                case "enterprise":
                    subscriptionTypeMessage = "Welcome to the Enterprise Subscription! Enjoy our premium features.";
                    break;
                default:
                    subscriptionTypeMessage = "Welcome to 23.1 News!";
                    break;
            }

            // Determine if subscription ends in two days
            DateTime subscriptionEndDate = newSubscription.Created.AddDays(30); // Assuming subscription duration is 30 days

            // Check if the subscription has reached its end date
            if (DateTime.Now.AddDays(2) >= subscriptionEndDate)
            {
                // Update subscription status to inactive
                newSubscription.IsActive = false;

                // Subscription ends in two days, add an additional message
                subscriptionTypeMessage += $" Your subscription is ending in two days. Renew now to continue enjoying our services!";
            }

            // Include subscriber's name in the email content
            string subscriberName = $"{newSubscription.User.FirstName} {newSubscription.User.LastName}";
            newEmail.Content = $"{subscriptionTypeMessage} Thank you, {subscriberName}, for subscribing.";

            newEmail.ToAddresses.Add(new EmailAddress()
            {
                Address = newSubscription.User.Email,
                Name = subscriberName
            });

            // Update the subscription status in the database
            _subscriptionService.UpdateSubs(newSubscription);

            _emailHelper.SendEmail(newEmail);

            return View();
        }



        [Authorize(Roles = "Editor, Admin")]

        //public IActionResult SubscriptionStatistics()
        //{
        //    var totalSubscribers = _subscriptionService.GetAllSubs().Count();
        //    var activeSubscribers = _subscriptionService.GetActiveSubscribersCount(); // You need to implement this method in your SubscriptionService

        //    var viewModel = new SubscriptionStatisticsVM
        //    {
        //        TotalSubscribers = totalSubscribers,
        //        ActiveSubscribers = activeSubscribers
        //    };

        //    return View(viewModel);
        //}

        public IActionResult SubscriptionStatistics()
        {
            var totalSubscribers = _subscriptionService.GetAllSubs().Count();
            var activeSubscribers = _subscriptionService.GetActiveSubscribersCount();

            // Assuming you have a method to get weekly subscription data from your service
            var weeklySubscriptionData = _subscriptionService.GetWeeklySubscriptionData();

            var viewModel = new SubscriptionStatisticsVM
            {
                TotalSubscribers = totalSubscribers,
                ActiveSubscribers = activeSubscribers,
                InActiveSubscribers = totalSubscribers - activeSubscribers,

                // Populate these based on your weekly data
                //WeeklyLabels = weeklySubscriptionData.Select(entry => entry.WeekLabel).ToList(),
                WeeklySubscribers = weeklySubscriptionData.Select(entry => entry.SubscriberCount).ToList(),

                SubscriptionTypes = _subscriptionService.GetSubscriptionTypes()
            };

            // Assuming you want to render a doughnut chart
            ViewBag.ChartType = "doughnut";

            return View(viewModel);
        }

        public IActionResult SubscribeToType(int subscriptionTypeId)
        {
            // Get the current user
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Create a new subscription with the obtained user identifier
            var newSubscription = new Subscription
            {
                SubscriptionTypeId = subscriptionTypeId,
                // Other subscription properties...
                User = currentUser
            };

            // Call the CreateSubs method with the new subscription
            _subscriptionService.CreateSubs(newSubscription);

            // Redirect or return a response as needed
            // ...

            return RedirectToAction("Index");
        }



    }
}




