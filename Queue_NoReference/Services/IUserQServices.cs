using Queue_NoReference.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue_NoReference.Services
{
    public interface IUserQServices
    {
        List<UserQ> GetAll(List<string> categoryNames);
    }
}
