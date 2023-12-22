using Microsoft.AspNetCore.Identity;

namespace _23._1News.Models.Db
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int DOB { get; set; }

        public string Address { get; set; } = string.Empty;

        //public int SelectedCategoryId { get; set; }  // For the selection of Newsletter categories 

        public virtual ICollection<Subscription> Subscriptions { get; set; }


        //public virtual Category SelectedCategoryName { get; set; }



        public bool ReceiveNewsletters { get; set; } = true;

        public virtual ICollection<Category> UserCategories { get; set; }

    }
}
