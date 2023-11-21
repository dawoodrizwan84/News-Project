namespace _23._1News.Helpers
{
    public class EmailConfiguration: IEmailConfiguration
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort {  get; set; }
        public string SmtpUsername { get; set;} = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;

        public string SenderEmail { get; set;}=string.Empty;
        public string SenderName { get; set; } = string.Empty;


    }
}
