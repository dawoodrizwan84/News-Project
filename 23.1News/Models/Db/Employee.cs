using _23._1News.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _23._1News.Models.Db
{
    public class Employee
    {
        
        [Key]
        public int Id { get; set; }

        // Foreign key for the User relationship
        //[ForeignKey("User")]
        //public string UserId { get; set; }

        // Navigation property for the User relationship
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Role { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        // Navigation property for the Role relationship
        public virtual IdentityRole IdentityRole { get; set; }

        public User User { get; set; }




    }
}
