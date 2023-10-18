namespace _23._1News.Models
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime DateStamp { get; set; }
        public string LinkText { get; set; }
        public string Headline { get; set; }
        public string ContentSummary { get; set; }
        public string Content { get; set; }
        public string ImageLink { get; set; }
        public string Category { get; set; }
    }
}
