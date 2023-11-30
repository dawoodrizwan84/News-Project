using _23._1News.Models.Db;
using _23._1News.Models.View_Models;

namespace _23._1News.Services.Abstract
{
    public interface ISubscriptionService
    {
        List<Subscription> GetAllSubs();

        void CreateSubs(Subscription newSub);

        bool UpdateSubs(Subscription newSubs);

        bool DeleteSubs(int id);

        Subscription GetSubsById(int id);

        User GetUserById(string id);

        List<Subscription> GetSubsByUserId(string id);

        int GetActiveSubscribersCount();
        
    }
}
