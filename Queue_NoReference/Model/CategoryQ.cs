using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue_NoReference.Model
{
    public class CategoryQ
    {
        public string Name { get; set; }

        public virtual ICollection<UserQ> CategoryUsers { get; set; }
    }
}
