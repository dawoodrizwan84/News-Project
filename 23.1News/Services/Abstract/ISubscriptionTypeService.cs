using _23._1News.Models.Db;

namespace _23._1News.Services.Abstract
{
    public interface ISubscriptionTypeService
    {
        List<SubscriptionType> GetAllSubscriptionTypes();

        void AddSubscriptionType(SubscriptionType subscriptionType);

        bool UpdateSubscriptionType(SubscriptionType subscriptionType);
        bool DeleteSubscriptionType(int id);

        SubscriptionType GetSubscriptionTypeById(int id);
        IEnumerable<SubscriptionType> GetSubscriptionTypesForUpgrade(int? subscriptionTypeId);
    }
}

