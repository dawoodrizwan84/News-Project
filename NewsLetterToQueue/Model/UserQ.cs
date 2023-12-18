
using Microsoft.AspNetCore.Identity;
using NewsLetterToQueue.Model;

namespace NewsLetterToQueue.Model
{
    public class UserQ
    {

        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int DOB { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool ReceiveNewsletters { get; set; } = true;

        public virtual ICollection<CategoryQ> UserCategories { get; set; }
        

       
      }
       
}


public class CategoryQ
{
    public int Id { get; set; }
    public virtual ICollection<UserQ> CategoryUsers { get; set; }
}