using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Models.View_Models;
using _23._1News.Models.ViewModels;
using _23._1News.Services.Abstract;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using System;
using System.Diagnostics;
using _23._1News.Controllers;
using Microsoft.Extensions.Logging;

namespace _23._1News.Services.Implement
{
    public class SubscriptionService: ISubscriptionService
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ArticleController> _logger;
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionService(ApplicationDbContext db, UserManager<User> userManager, ILogger<ArticleController> logger)
        {
            _db = db;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger;
        }

        public List<SubscriptionListVM> GetAllSubs()
        {
            var subscriptions = _db.Subscriptions
                .Include(sub => sub.SubscriptionType)
                .Include(sub => sub.User)
                .OrderByDescending(sub => sub.Id)
                .Select(sub => new SubscriptionListVM
                {
                    SubscriptionId = sub.Id,
                    SubscriptionPrice = sub.Price,
                    SubscriptionCreated = sub.Created,
                    SubscriptionTypeId = sub.SubscriptionTypeId,
                    SubscriptionTypeName = sub.SubscriptionType.TypeName,
                    IsActive = sub.IsActive,
                    PaymentComplete = sub.PaymentComplete,
                    UserId = sub.UserId
                })
                .ToList();

            return subscriptions;
        }

        public void CreateSubs(Subscription newSub)
        {
            newSub.SubscriptionType = _db.SubscriptionTypes.Where(t => t.Id == newSub.SubscriptionTypeId).FirstOrDefault()!;
            newSub.Created = DateTime.Now;
            newSub.Price = newSub.SubscriptionType.Price;
            newSub.IsActive = true;
            _db.Subscriptions.Add(newSub);
            _db.SaveChanges();
        }

        public bool UpdateSubs(Subscription newSubs) 
        {
            try
            {
                _db.Subscriptions.Update(newSubs);
                _db.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public Subscription GetSubsById(int id)
        {
            return _db.Subscriptions.Find(id);
        }

        public User GetUserById(string id)
        {

            return _db.Users.Find(id);


        }

        public List<Subscription> GetSubsByUserId(string id)
        {
            var subscriptions = _db.Subscriptions.Where(Subscription => Subscription.UserId== id)
                            .OrderByDescending(a => a.Created).ToList();

            return subscriptions;
        }

        public bool DeleteSubs(int id)
        {
            try
            {
                var sub = this.GetSubsById(id);
                if (sub == null)
                {
                    return false;
                }
                _db.Subscriptions.Remove(sub);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public int GetActiveSubscribersCount()
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            return _db.Subscriptions.Count(s => s.Created >= thirtyDaysAgo);
        }


        //public Subscription GetActiveSubscriptionByUser(string userId)
        //{
        //    // Retrieve the active subscription for the given user
        //    var activeSubscription = _db.Subscriptions
        //        .Where(s => s.User.Id == userId && s.IsActive)
        //        .OrderByDescending(s => s.Created)
        //        .FirstOrDefault();

        //    return activeSubscription;
        //}

        public Subscription GetActiveSubscriptionByUser(string userId)
        {
            var activeSubscription = _db.Subscriptions
                .Include(s => s.User) 
                .Where(s => s.User != null && s.User.Id == userId && s.IsActive)
                .OrderByDescending(s => s.Created)
                .FirstOrDefault();

            return activeSubscription;
        }

        public IEnumerable<Subscription> GetWeeklySubscriptionData()
        {
            DateTime lastWeekStartDate = DateTime.Today.AddDays(-7);

            var weeklyData = _db.WeeklySubscriptionData
                .Where(weekly => weekly.Date >= lastWeekStartDate)
                .ToList();

            var subscriptions = weeklyData.Select(weekly =>
                new Subscription
                {
                    // Assign properties specific to Subscription, adapt this to your needs
                    WeekLabel = weekly.WeekLabel,
                    SubscriberCount = weekly.SubscriberCount,
                    // ...
                });

            return subscriptions;
        }

        public List<SubscriptionType> GetSubscriptionTypes()
        {
            return _db.SubscriptionTypes.ToList();
        }

        public bool IsEnterpriseUser(string userId)
        {
            try
            {
                var userSubscription = GetActiveSubscriptionByUserId(userId);

                return userSubscription?.SubscriptionType?.TypeName.ToLower() == "enterprise";
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        public Subscription GetActiveSubscriptionByUserId(string userId)
        {
            return _db.Subscriptions
                .Where(s => s.UserId == userId && s.IsActive)
                .OrderByDescending(s => s.Created)
                .FirstOrDefault();
        }

        public async Task<bool> UpgradeSubscriptionAsync(string userId, int newSubscriptionTypeId)
        {
            try
            {
                var activeSubscription = GetActiveSubscriptionByUserId(userId);

                if (activeSubscription == null)
                {
                    // User doesn't have an active subscription
                    return false;
                }

                if (CanUpgradeSubscription(activeSubscription.SubscriptionTypeId, newSubscriptionTypeId))
                {
                    // Deactivate the current subscription
                    activeSubscription.IsActive = false;
                    _db.Subscriptions.Update(activeSubscription);
                    await _db.SaveChangesAsync();

                    // Create a new subscription with the specified type
                    var newSubscription = new Subscription
                    {
                        SubscriptionTypeId = newSubscriptionTypeId,
                        User = _db.Users.Find(userId),
                        Created = DateTime.Now,
                        Price = _db.SubscriptionTypes.Find(newSubscriptionTypeId)?.Price ?? 0,
                        IsActive = true
                    };

                    _db.Subscriptions.Add(newSubscription);
                    await _db.SaveChangesAsync();

                    return true;
                }

                // Invalid upgrade
                return false;
            }
            catch (Exception)
            {
                // Log or handle the exception as needed
                throw;
            }
        }

        private bool CanUpgradeSubscription(int currentSubscriptionTypeId, int newSubscriptionTypeId)
        {
            // Example: Allow upgrading from Free to Pro or Enterprise, and from Pro to Enterprise
            return newSubscriptionTypeId != 11 &&
                   ((currentSubscriptionTypeId == 1 && newSubscriptionTypeId == 2) ||
                    (currentSubscriptionTypeId == 1 && newSubscriptionTypeId == 3) ||
                    (currentSubscriptionTypeId == 2 && newSubscriptionTypeId == 3));
        }

        public IEnumerable<SubscriptionType> GetSubscriptionTypesForUpgrade(int currentSubscriptionTypeId)
        {
            // Retrieve all subscription types that have a higher ID than the current subscription type
            return _db.SubscriptionTypes
                .Where(type => type.Id > currentSubscriptionTypeId)
                .ToList();
        }

        public bool isEnteprise(string userId)
        {
            try
            {
                var userSubscription = GetActiveSubscriptionByUserId(userId);
                // Assuming there is a property like SubscriptionType in your Subscription model

                return userSubscription?.SubscriptionType?.TypeName.ToLower() == "enterprise";
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        bool ISubscriptionService.CanUpgradeSubscription(int subscriptionTypeId, int newSubscriptionTypeId)
        {
            bool canUpgrade = CanUpgrade(subscriptionTypeId, newSubscriptionTypeId);

            return canUpgrade;
        }

        private bool CanUpgrade(int subscriptionTypeId, int newSubscriptionTypeId)
        {
            int currentSubscriptionType = 0;
            if (newSubscriptionTypeId > currentSubscriptionType)
            {
                // Upgrade is allowed.
                return true;
            }
            else
            {
                // Upgrade is not allowed.
                return false;
            }
        }

        public bool IsSubscribedToNewsletter(string userId)
        {
            try
            {

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ResetPasswordAsync(string userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    // User not found
                    return false;
                }

                // Check if the old password is correct
                var passwordCheck = await _userManager.CheckPasswordAsync(user, oldPassword);

                if (!passwordCheck)
                {
                    // Old password is incorrect
                    return false;
                }

                // Change the password
                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                return result.Succeeded;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        public bool IsUnSubscribedFromNewsletter(string userId, string NewsletterSubscriptionStatus)
        {
            try
            {
                // Logic to check if the user is unsubscribed from the newsletter
                // For example, query the user's record in the database

                // For demonstration purposes, let's assume there is a User entity
                var user = _db.Users.Find(userId);

                if (user != null)
                {
                    var isUnsubscribed = !_subscriptionService.IsSubscribedToNewsletter(userId);

                    // Set the NewsletterSubscriptionStatus based on the subscription status
                    if (isUnsubscribed)
                    {
                        NewsletterSubscriptionStatus = "You are currently unsubscribed from the newsletter.";
                    }
                    else
                    {
                        NewsletterSubscriptionStatus = "You are subscribed to the newsletter.";
                    }

                    return isUnsubscribed;
                }

                // Log an error if the user is not found
                _logger.LogError($"User with ID {userId} not found when checking newsletter subscription status.");
                NewsletterSubscriptionStatus = "Error checking subscription status.";
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Error checking newsletter subscription status for user with ID {userId}: {ex.Message}");
                NewsletterSubscriptionStatus = "Error checking subscription status.";
                // You might want to rethrow the exception here if you want to propagate it further
                return false;
            }
        }

        public bool NewsletterSubscriptionStatus(string userId)
        {
            return _db.Users.Any(x => !x.ReceiveNewsletters && x.Id == userId);
        }

        public bool IsUnSubscribedFromNewsletter(string userId)
        {
            return _db.Users.Any(x => !x.ReceiveNewsletters && x.Id == userId);
        }

        public bool SubscribeToNewsletter(string userId)
        {
            try
            {

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UnsubscribeFromNewsletter(string userId)
        {
            try
            {

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}


