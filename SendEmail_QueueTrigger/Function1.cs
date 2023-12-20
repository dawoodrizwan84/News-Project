using System;
using System.Net;
using System.Net.Mail;
using _23._1News.Models.Db;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace SendEmail_QueueTrigger
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public void Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] User user)
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
                mailMessage.From = new MailAddress(configuration["senderemailservice23.1@gmail.com"],
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
                smtpClient.Send(mailMessage);


            }
            catch (Exception ex)
            {

                _logger.LogError($"Error sending email: {ex}");
                throw;
            }
        }
    }
}
