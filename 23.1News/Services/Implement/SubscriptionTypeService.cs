using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Azure.Storage.Blobs.Models;

namespace _23._1News.Services.Implement
{
    public class SubscriptionTypeService : ISubscriptionTypeService
    {
        private readonly ApplicationDbContext _db;


        public SubscriptionTypeService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<SubscriptionType> GetAllSubscriptionTypes()
        {
            return _db.SubscriptionTypes.ToList();
        }


        public void AddSubscriptionType(SubscriptionType subscriptionType)
        {
            _db.SubscriptionTypes.Add(subscriptionType);
            _db.SaveChanges();
        }

        public bool UpdateSubscriptionType(SubscriptionType subscriptionType)
        {
            try
            {
                _db.SubscriptionTypes.Update(subscriptionType);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public SubscriptionType GetSubscriptionTypeById(int id)
        {
            return _db.SubscriptionTypes.Find(id);
        }

        public bool DeleteSubscriptionType(int id)
        {
            try
            {
                var data = this.GetSubscriptionTypeById(id);
                if (data == null)
                {
                    return false;
                }
                _db.SubscriptionTypes.Remove(data);
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

