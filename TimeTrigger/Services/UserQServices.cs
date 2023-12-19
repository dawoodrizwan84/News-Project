using TimeTrigger.Data;
using TimeTrigger.Model;
using TimeTrigger.Services;

namespace Queue_NoReference.Services
{
    public class UserQServices : IUserQServices
    {
        private readonly AppDbContext _appDbContext;


        public UserQServices(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }

        public List<UserQ> GetAll(List<string> categoryNames)
        {

            return _appDbContext.userQ  
        .Where(u => u.UserCategories == u.UserCategories)
        .ToList();
        }
    }
}
