using System.Net;
using System.Net.Mail;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace QueueTrigger
{
    public class SendEmail
    {
        private readonly ILogger _logger;
      
              
        public SendEmail(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SendEmail>();
          
        }

        [Function("SendEmail")] 
        public void Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] User user, 
            ILogger logger)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {user.Email}");

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json", true, true)
                    .AddEnvironmentVariables()
                    .Build();

            MailMessage mailMessage = new MailMessage();

            SmtpClient smtpClient = new SmtpClient(configuration["SmtpHost"]);


            int port = int.Parse(configuration["SmtpPort"]);

            try
            {
                mailMessage.From = new MailAddress(configuration["EmailAddress"],
                                configuration["23.1News"]);

                mailMessage.To.Add(user.Email);
                mailMessage.Subject = "Your weekly Newsletter!";
                mailMessage.Body = "<p> On " + DateTime.Now.AddDays(5).ToLongDateString()
                                             + $"Good Afternoon {user.FirstName}!<br> " +
                                             $"Article of your choice:";
                mailMessage.IsBodyHtml = true;
                smtpClient.Port = port;
                smtpClient.Credentials = new NetworkCredential(configuration["SmtpServer"],
                                        configuration["SmtpPassword"]);
                //smtpClient.Send(mailMessage);


            }
            catch (Exception ex)
            {

                _logger.LogError($"Error sending email: {ex}");
                throw;
            }
        }
    }
}


public class User
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}