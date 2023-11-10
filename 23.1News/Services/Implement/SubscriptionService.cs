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

        
        
        public List<Subscription> GetAllSubs()
        {
            return _db.Subscriptions.ToList();
        }

        public void CreateSubs(Subscription newSub) 
        {
            newSub.Created = DateTime.Now;
            _db.Subscriptions.Add(newSub);
            _db.SaveChanges();
        }

        public bool UpdateSubs(Subscription newSubs) 
        {
            try
            {
                _db.Subscriptions.Update(newSubs);
                _db.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public Subscription GetSubsById(int id)
        {
            return _db.Subscriptions.Find(id);
        }

        public bool DeleteSubs(int id)
        {
            try
            {
                var sub = this.GetSubsById(id);
                if (sub == null)
                {
                    return false;
                }
                _db.Subscriptions.Remove(sub);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }

}
