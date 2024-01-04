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
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;

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


        [HttpPost]
        public IActionResult Create(Subscription newSubscription)
        {
            // Get the current user
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Check if the user is already subscribed
            var existingSubscription = _subscriptionService.GetActiveSubscriptionByUser(currentUser.Id);

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
                WeeklyLabels = weeklySubscriptionData.Select(entry => entry.WeekLabel).ToList(),
                WeeklySubscribers = weeklySubscriptionData.Select(entry => entry.SubscriberCount).ToList(),

                SubscriptionTypes = _subscriptionService.GetSubscriptionTypes()
            };

            // Assuming you want to render a doughnut chart
            ViewBag.ChartType = "doughnut";

            return View(viewModel);
        }


        [Authorize]
        public IActionResult MyPage()
        {
            var userId = _userManager.GetUserId(User);
            var activeSubscription = _subscriptionService.GetActiveSubscriptionByUser(userId);

            var viewModel = new MyPageVM
            {
                IsEnterpriseUser = _subscriptionService.IsEnterpriseUser(userId),
                IsSubscribedToNewsletter = _subscriptionService.IsSubscribedToNewsletter(userId),
                ActiveSubscriptionType = activeSubscription?.SubscriptionType?.TypeName // Include the active subscription type
            };

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

        [Authorize]
        public IActionResult UpgradeSubscription()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            var activeSubscription = _subscriptionService.GetActiveSubscriptionByUser(currentUser.Id);

            
            if (_subscriptionTypeService != null)
            {
                var availableTypes = _subscriptionTypeService.GetSubscriptionTypesForUpgrade(activeSubscription?.SubscriptionTypeId);
                var viewModel = new UpgradeSubscriptionVM
                {
                    ActiveSubscription = activeSubscription,
                    AvailableTypes = availableTypes
                };

                return View(viewModel);
            }
            else
            {
                // Handle the case where _subscriptionTypeService is not available
                // You can log an error, return an error view, or take other appropriate actions
                return View("Error");
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpgradeSubscription(int newSubscriptionTypeId)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);

                // Get the active subscription
                var activeSubscription = _subscriptionService.GetActiveSubscriptionByUser(currentUser.Id);

                if (activeSubscription != null)
                {
                    // Check if the upgrade is valid
                    if (_subscriptionService.CanUpgradeSubscription(activeSubscription.SubscriptionTypeId, newSubscriptionTypeId))
                    {
                        // Deactivate the current subscription
                        activeSubscription.IsActive = false;
                        _applicationDbContext.Subscriptions.Update(activeSubscription);
                        await _applicationDbContext.SaveChangesAsync();

                        // Create a new subscription with the specified type
                        var newSubscription = new Subscription
                        {
                            SubscriptionTypeId = newSubscriptionTypeId,
                            User = currentUser,
                            Created = DateTime.Now,
                            Price = _applicationDbContext.SubscriptionTypes.Find(newSubscriptionTypeId)?.Price ?? 0,
                            IsActive = true
                        };

                        _applicationDbContext.Subscriptions.Add(newSubscription);
                        await _applicationDbContext.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Subscription upgraded successfully!";

                        // Send email notification
                        SendSubscriptionUpgradeEmail(currentUser, activeSubscription, newSubscription);

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid upgrade. Please try again.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "User does not have an active subscription.";
                }

                return RedirectToAction("MyPage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error upgrading subscription");
                TempData["ErrorMessage"] = "An error occurred while upgrading the subscription.";
                return RedirectToAction("MyPage");
            }
        }

        private void SendSubscriptionUpgradeEmail(User user, Subscription oldSubscription, Subscription newSubscription)
        {
           
            EmailMessage upgradeEmail = new EmailMessage
            {
                FromAddress = new EmailAddress
                {
                    Address = "senderemailservice23.1@gmail.com",
                    Name = "23.1News"
                },
                Subject = "Subscription Upgrade Notification"
            };


            string upgradeMessage = $"Hello {user?.FirstName},\n\n";

            if (oldSubscription != null && newSubscription != null && oldSubscription.SubscriptionType != null && newSubscription.SubscriptionType != null)
            {
                upgradeMessage += $"Your subscription has been upgraded from {oldSubscription.SubscriptionType.TypeName} to {newSubscription.SubscriptionType.TypeName}.\n";
            }
            else
            {
                upgradeMessage += "Your subscription has been upgraded.\n";
            }

            upgradeMessage += "Thank you for choosing our services!";

            upgradeEmail.Content = upgradeMessage;

           
            upgradeEmail.ToAddresses.Add(new EmailAddress
            {
                Address = user.Email,
                Name = $"{user.FirstName} {user.LastName}"
            });

            
            _emailHelper.SendEmail(upgradeEmail);
        }



        [Authorize]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            var result = _subscriptionService.ResetPasswordAsync(userId, model.OldPassword, model.NewPassword).Result;

            if (result)
            {
                TempData["SuccessMessage"] = "Password successfully changed!";
                return RedirectToAction("MyPage");
            }
            else
            {
                ModelState.AddModelError("", "Failed to reset password. Please check your old password.");
                return View(model);
            }
        }

        [Authorize]
        public IActionResult ManageNewsletter()
        {
            var userId = _userManager.GetUserId(User);
            var isSubscribed = _subscriptionService.IsSubscribedToNewsletter(userId);
            var isUnSubscribed = _subscriptionService.IsUnSubscribedFromNewsletter(userId);

            var viewModel = new ManageNewsletterVM
            {
                IsSubscribedToNewsletter = isSubscribed,
                IsSubscribedFromNewsletter = isUnSubscribed
            };

            return View(viewModel);
        }

        public IActionResult SendNewsletterSubscriptionEmail(User user, bool isSubscribed)
        {
            EmailMessage email = new EmailMessage()
            {
                FromAddress = new EmailAddress()
                {
                    Address = "senderemailservice23.1@gmail.com",
                    Name = "23.1News"
                },
                Subject = isSubscribed ? "Welcome to Our Newsletter" : "Unsubscribed from Our Newsletter"
            };

            string subscriptionMessage = isSubscribed
                ? "Thank you for subscribing to our newsletter! Stay tuned for the latest updates."
                : "You have unsubscribed from our newsletter. We're sorry to see you go.";

            // Include user's name in the email content
            string userName = $"{user.FirstName} {user.LastName}";
            email.Content = $"{subscriptionMessage} Thank you, {userName}, for your continued support.";

            email.ToAddresses.Add(new EmailAddress()
            {
                Address = user.Email,
                Name = userName
            });

            // Additional logic or customization can be added here

            _emailHelper.SendEmail(email);

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubscribeToNewsletter()
        {
            var userId = _userManager.GetUserId(User);
            var result = _subscriptionService.SubscribeToNewsletter(userId);

            if (result)
            {
                // Send subscription email
                var user = _userManager.FindByIdAsync(userId).Result;
                SendNewsletterSubscriptionEmail(user, isSubscribed: true);

                TempData["SuccessMessage"] = "Subscribed to the newsletter!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to subscribe to the newsletter.";
            }

            return RedirectToAction("ManageNewsletter");
        }

        [Authorize]
        [HttpPost]
        public IActionResult UnsubscribeFromNewsletter()
        {
            var userId = _userManager.GetUserId(User);
            var result = _subscriptionService.UnsubscribeFromNewsletter(userId);

            if (result)
            {
                // Send unsubscription email
                var user = _userManager.FindByIdAsync(userId).Result;
                SendNewsletterSubscriptionEmail(user, isSubscribed: false);

                TempData["SuccessMessage"] = "Unsubscribed from the newsletter!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to unsubscribe from the newsletter.";
            }

            return RedirectToAction("ManageNewsletter");
        }

    }
}




