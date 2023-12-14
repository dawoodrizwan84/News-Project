using System;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Bcpg;

namespace SendNewsLetterGmail

{
    public class SendGmail
    {

        private readonly ILogger<SendGmail> _logger;
        private readonly IConfiguration _configuration;


        public SendGmail(ILogger<SendGmail> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [FunctionName("SendGmail")]
        public void Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] User user
            , ILogger log)
        {

            log.LogInformation($"C# Queue trigger function processed: {user.Email}");



            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json", true, true)
                    .AddEnvironmentVariables()
                    .Build();



            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("23.1News", _configuration["EmailAddress"]));
                message.To.Add(new MailboxAddress(user.FirstName, user.Email));
                message.Subject = "Weekly Newsletter";
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<p> On " + DateTime.Now.AddDays(5).ToLongDateString() +
                            $" Hello {user.FirstName}! <br/>" +
                            $"\"Your Weekly Newsletter of Choice: {user.SelectedCategoryId} <br/>" +
                            "Thankyou for subscribing. </p>"
                };

                using (var emailClient = new SmtpClient())
                {
                    emailClient.Connect(_configuration["SmtpServer"], int.Parse(_configuration["SmtpPort"]), SecureSocketOptions.StartTls);
                    emailClient.Authenticate(_configuration["EmailAddress"], _configuration["SmtpPassword"]);
                    emailClient.Send(message);
                    emailClient.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log or rethrow if necessary)
                _logger.LogError($"An error occurred: {ex.Message}");


            }

        }



    }

    public class User
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SelectedCategoryId { get; set; }
    }
}
