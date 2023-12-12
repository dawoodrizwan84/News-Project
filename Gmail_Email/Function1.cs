using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gmail_Email
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public void Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] myQueueItem myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem.Email}");

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

                mailMessage.To.Add(myQueueItem.Email);
                mailMessage.Subject = "Your weekly Newsletter!";
                mailMessage.Body = "<p> On " + DateTime.Now.AddDays(5).ToLongDateString()
                                             + $"Good Afternoon {myQueueItem.FirstName}!<br> " +
                                             $"Article of your choice:";
                mailMessage.IsBodyHtml = true;
                smtpClient.Port = port;
                smtpClient.Credentials = new NetworkCredential(configuration["SmtpServer"],
                                        configuration["SmtpPassword"]);
                smtpClient.Port = port;
                smtpClient.EnableSsl = true; // Enable SSL/TLS
                smtpClient.Credentials = new NetworkCredential(configuration["SmtpUsername"], configuration["SmtpPassword"]);
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



public class myQueueItem
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}