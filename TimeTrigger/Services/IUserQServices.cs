using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrigger.Model;

namespace TimeTrigger.Services
{
    public interface IUserQServices
    {
        List<UserQ> GetAll(List<string> categoryNames);
    }
}
