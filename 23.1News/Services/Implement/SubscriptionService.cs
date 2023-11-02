using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;

namespace _23._1News.Services.Implement
{
    public class SubscriptionService: ISubscriptionService
    {

        private readonly ApplicationDbContext _db;
       
        public SubscriptionService(ApplicationDbContext db)
        {
            _db = db;
           
        }


        public List<Subscription> GetAllSubscription()
        {
            return _db.Subscriptions.ToList();
        }

        public void CreateSubs(Subscription newsubscription) 
        {
            _db.Subscriptions.Add(newsubscription);
        }
    }

}
