using Microsoft.AspNetCore.Identity;

namespace _23._1News.Models.Db
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int DOB { get; set; }

        public string Employee { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
