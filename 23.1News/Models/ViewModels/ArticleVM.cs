using _23._1News.Models.Db;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _23._1News.Models.View_Models
{
    public class ArticleVM
    {

        public ArticleVM()
        {
            Categories = new List<SelectListItem>();
        }


        //public int Id { get; set; }

        [Display(Name = "Date")]

        public DateTime DateStamp { get; set; } = DateTime.Now;

        [Display(Name = "Text To Link")]

        public string LinkText { get; set; } = string.Empty;


        [Display(Name = "Headline")]

        public string Headline { get; set; }


        [Display(Name = "Summary")]
        [StringLength(200)]
        public string ContentSummary { get; set; }


        [Display(Name = "Content")]

        public string Content { get; set; }

        public string ImageLink { get; set; }

        
        public Category Category { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public string ChosenCategory { get; set; }
        public IFormFile File { get; set; }


    }
}
