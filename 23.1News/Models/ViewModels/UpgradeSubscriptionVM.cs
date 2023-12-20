using _23._1News.Models.Db;

namespace _23._1News.Models.ViewModels
{
    public class UpgradeSubscriptionVM
    {
        public Subscription ActiveSubscription { get; set; }
        public IEnumerable<SubscriptionType> AvailableTypes { get; set; }
        public int NewSubscriptionTypeId { get; set; }
        public string? CurrentSubscriptionType { get; internal set; }
    }
}
