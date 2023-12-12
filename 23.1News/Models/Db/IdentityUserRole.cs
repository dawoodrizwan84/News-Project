using Microsoft.AspNet.Identity.EntityFramework;

namespace _23._1News.Models.Db
{
    public class IdentityUserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
    }
}
