using Queue_NoReference.Data;
using Queue_NoReference.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue_NoReference.Services
{
    public class UserQServicescs : IUserQServices
    {
        private readonly AppDbContext _appDbContext;


        public UserQServicescs(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }

        public List<UserQ> GetAll(List<string> categoryNames)
        {

            return _appDbContext.userQ  
        .Where(u => u.UserCategories.Any(c => categoryNames.Contains(c.Name)))
        .ToList();
        }
    }
}
