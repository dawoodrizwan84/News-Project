using System.ComponentModel.DataAnnotations;

namespace TimeTrigger.Model
{

    public class CategoryQ
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }

        //public virtual ICollection<UserQ> CategoryUsers { get; set; }
       
        //public bool IsSelected { get; internal set; }
    }
}
