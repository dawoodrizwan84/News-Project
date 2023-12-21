using _23._1News.Models.Db;
using _23._1News.Models.View_Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace _23._1News.Services.Abstract
{
    public interface ISubscriptionService
    {

        List<SubscriptionListVM> GetAllSubs();

        void CreateSubs(Subscription newSub);

        bool UpdateSubs(Subscription newSubs);

        bool DeleteSubs(int id);

        Subscription GetSubsById(int id);

        User GetUserById(string id);

        List<Subscription> GetSubsByUserId(string id);

        int GetActiveSubscribersCount();

        Subscription GetActiveSubscriptionByUser(string Id);

        //IEnumerable<object> GetWeeklySubscriptionData();

        IEnumerable<Subscription> GetWeeklySubscriptionData();
        List<SubscriptionType> GetSubscriptionTypes();

        // object GetSubsByUserId(string? userId);
        bool isEnteprise(string userId);
        Task<bool> UpgradeSubscriptionAsync(string userId, int newSubscriptionTypeId);
        IEnumerable<SubscriptionType> GetSubscriptionTypesForUpgrade(int SubscriptionTypeId);
        bool IsSubscribedToNewsletter(string userId);
        bool IsEnterpriseUser(string userId);
        bool CanUpgradeSubscription(int subscriptionTypeId, int newSubscriptionTypeId);
        Task<bool> ResetPasswordAsync(string userId, string oldPassword, string newPassword);
        bool SubscribeToNewsletter(string userId);
        bool UnsubscribeFromNewsletter(string userId);
        bool IsUnSubscribedFromNewsletter(string userId);
        //bool NewsletterSubscriptionStatus(string userId);

    }
}
