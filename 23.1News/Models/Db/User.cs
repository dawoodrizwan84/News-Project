
using Microsoft.AspNetCore.Identity;

namespace _23._1News.Models.Db
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        //public bool Employee { get; set; }
        public int DOB { get; set; }

        public string Address { get; set; } = string.Empty;

        public virtual ICollection<Subscription> Subscriptions { get; set; }

    }
}
