namespace _23._1News.Models.Db
{
    public class NewsletterSubscription
    {
        public int Id { get; set; }


        public virtual User User { get; set; }


        public bool IsSubscribed { get; set; }


        public DateTime LastUpdated { get; set; }


        public bool ReceiveNewsletters { get; set; } = true;
    }
}
