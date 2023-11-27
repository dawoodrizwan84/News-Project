using System;
using System.Net;
using System.Net.Mail;
using _23._1News.Models.Db;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QueueTrigger
{
    public class SocketLab
    {
        private readonly ILogger _logger;

        public SocketLab(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SocketLab>();
        }

        [Function("SocketLab")]
        public void Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] User user)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {user.Email}");

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json", true, true)
                    .AddEnvironmentVariables()
                    .Build();

            MailMessage mailMessage = new MailMessage();

            SmtpClient smtpClient = new SmtpClient(configuration["Host"]);
            int port = int.Parse(configuration["port"]);

                try
                {
                    mailMessage.From = new MailAddress(configuration["robert@dreammaker-it.se"],
                                    configuration["23.1News"]);

                    mailMessage.To.Add(user.Email);
                    mailMessage.Subject = "Your weekly Newsletter!";
                    mailMessage.Body = "<p> On " + DateTime.Now.AddDays(5).ToLongDateString()
                                                 + $"Good Afternoon {user.FirstName}!<br> " +
                                                 $"Article of your chouce.";
                    mailMessage.IsBodyHtml = true;
                    smtpClient.Port = port;
                    smtpClient.Credentials = new NetworkCredential(configuration["Server"],
                                            configuration["Password"]);
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
