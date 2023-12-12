namespace _23._1News.Helpers
{
    public interface IEmailConfiguration
    {
        string SmtpServer { get; }
        public int SmtpPort { get; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }   

        public string SenderEmail {  get; set; }
        
        public string SenderName { get; set; }
        
    }
}
