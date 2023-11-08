using System.ComponentModel.DataAnnotations;

namespace _23._1News.Models.Email
{
    public class EmailAddress
    {
        public string Name {  get; set; }=string.Empty;

        [EmailAddress]
        public string Address { get; set; } = string.Empty;
    }
}
