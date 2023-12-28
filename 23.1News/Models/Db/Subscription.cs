using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? WeekLabel { get; set; }   
        public int SubscriberCount { get; set; }    

        //public User User { get; set; }
        public string? UserId { get; set; }
        public virtual User User { get; set; }

        //public string UserIdentifier { get; set; } // Assuming this is the user identifier property

        [NotMapped] // Add this attribute to exclude the property from being mapped to the database
        public ICollection<WeeklySubscriptionData> WeeklyData { get; set; }
        //public DateTime ExpiryDate { get; set; }
    }
}
