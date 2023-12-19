

using Microsoft.AspNetCore.Identity;

namespace TimeTrigger.Model
{
    public class UserQ : IdentityUser
    {
       
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public virtual ICollection<CategoryQ> UserCategories { get; set; }
        

    }
}
