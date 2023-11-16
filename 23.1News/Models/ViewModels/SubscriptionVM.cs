using _23._1News.Models.Db;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _23._1News.Models.ViewModels
{
    
    public class SubscriptionVM
    {
       public List<User> User { get; set; }
        public List<SubscriptionType> SubscriptionTypes { get; set; }
       public List<SubscriptionType> TypeName { get; set; } = new List<SubscriptionType>();
        public List<SubscriptionType> Description { get; set; }
        public List<SubscriptionType> Price { get; set; }
        public DateTime Created { get; set; }

        public bool PaymentComplete { get; set; }

       
        public string Email {  get; set; }

    }
}
