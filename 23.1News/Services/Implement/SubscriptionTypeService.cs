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

        public IEnumerable<SubscriptionType> GetSubscriptionTypesForUpgrade(int? subscriptionTypeId)
        {
            try
            {
                // Assuming you have some logic to determine available subscription types for an upgrade
                // In this example, it returns all subscription types except the current one
                var allSubscriptionTypes = _db.SubscriptionTypes.ToList();

                if (subscriptionTypeId.HasValue)
                {
                    // Filter out the current subscription type
                    return allSubscriptionTypes.Where(st => st.Id != subscriptionTypeId.Value);
                }
                else
                {
                    return allSubscriptionTypes;
                }
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
    }
}

