using _23._1News.Data;
using _23._1News.Models.Db;

namespace NewsLetterToQueue.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserService(ApplicationDbContext applicationDbContext)
        {
          
            _applicationDbContext = applicationDbContext;
        }

        public List<User> GetAll()
        {
            var usa = _applicationDbContext.Users.ToList();
            return usa;
            
        }
    }
}
