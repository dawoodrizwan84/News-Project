using _23._1News.Models.Db;

namespace _23._1News.Models.ViewModels
{
    public class NewsletterSubscriptionVM
    {
        public string UserId { get; set; }
        public bool IsSubscribed { get; set; }
        public User User { get; set; }

        public int Id { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool ReceiveNewsletters { get; set; } = true;
    }
}
