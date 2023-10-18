namespace _23._1News.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public User User { get; set; }
        public bool PaymentComplete { get; set; }
    }
}
