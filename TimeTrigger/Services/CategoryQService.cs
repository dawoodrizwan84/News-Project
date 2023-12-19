
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrigger.Data;
using TimeTrigger.Model;

namespace TimeTrigger.Services
{
    public class CategoryQService : ICategoryQService
    {
        private readonly AppDbContext _db;

        public CategoryQService(AppDbContext db)
        {
                _db = db;
        }

       
    }
}
