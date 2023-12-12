using _23._1News.Models.Db;

namespace _23._1News.Models.ViewModels
{
    public class SubscriptionStatisticsVM
    {
        public int TotalSubscribers { get; set; }
        public int ActiveSubscribers { get; set; }
        public int InActiveSubscribers { get; set; }
        public int SubscriberCount { get; set; }

        // Add these two properties for weekly subscriptions
        public List<string> WeeklyLabels { get; set; }
        public List<int> WeeklySubscribers { get; set; }
        public List<SubscriptionType> SubscriptionTypes { get; set; }

       
    }
}
