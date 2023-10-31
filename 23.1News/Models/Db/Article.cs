using _23._1News.Models.View_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Graph;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _23._1News.Models.Db
{
    public class Article
    {

        public int Id { get; set; }

        [Display(Name = "Date")]

        public DateTime DateStamp { get; set; } = DateTime.Now;

        [Display(Name = "Text To Link")]

        public string LinkText { get; set; }


        [Display(Name = "Headline")]

        public string Headline { get; set; }


        [Display(Name = "Summary")]
        [StringLength(200)]
        public string ContentSummary { get; set; }


        [Display(Name = "Content")]

        public string Content { get; set; }

        public string ImageLink { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public User Author { get; set; }
             

    }

}
