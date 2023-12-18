using _23._1News.Models.Db;
using NewsLetterToQueue.Model;

namespace NewsLetterToQueue.Services
{
    public interface IUserService
    {
        List<User> GetAll();
    }
}
