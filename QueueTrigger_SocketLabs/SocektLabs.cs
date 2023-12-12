using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using _23._1News.Models.Db;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QueueTrigger_SocketLabs
{
    public class SocektLabs
    {
        [FunctionName("SocektLabs")]
        public void Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] User user, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {user.Email}");

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
                mailMessage.From = new MailAddress(configuration["EmailAddress"],
                                configuration["23.1News"]);

                mailMessage.To.Add(user.Email);
                mailMessage.Subject = "Your weekly Newsletter!";
                mailMessage.Body = "<p> On " + DateTime.Now.AddDays(5).ToLongDateString()
                                             + $"Good Afternoon {user.FirstName}!<br> " +
                                             $"Article of your choice.";
                mailMessage.IsBodyHtml = true;
                smtpClient.Port = port;
                smtpClient.Credentials = new NetworkCredential(configuration["Server"],
                                        configuration["Password"]);
                smtpClient.Send(mailMessage);


            }
            catch (Exception ex)
            {

                log.LogError($"Error sending email: {ex}");
                throw;
            }

        }
    }
}
