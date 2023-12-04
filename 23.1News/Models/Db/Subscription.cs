using Microsoft.AspNetCore.Identity;

namespace _23._1News.Models.Db
{
    public class Subscription
    {
        public int Id { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int SubscriptionTypeId { get; set; }
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
        public bool PaymentComplete { get; set; }

        //public User User { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
