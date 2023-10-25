namespace _23._1News.Models.Db
{
    public class Subscription
    {
        public int Id { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
       
        public User User { get; set; }
        public bool PaymentComplete { get; set; }
    }
}
