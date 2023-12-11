using System;
using System.IO;
using MailKit.Net.Smtp;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace ExpiryEmail
{
    public class EmailOnExpiry
    {

        private readonly ILogger<EmailOnExpiry> _logger;
        private readonly IConfiguration _configuration;

        public EmailOnExpiry(ILogger<EmailOnExpiry> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [FunctionName("EmailOnExpiry")]
        public void Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")]User user
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
                message.From.Add(new MailboxAddress("23.1News", _configuration["EmailConfiguration:SmtpUsername"]));
                message.To.Add(new MailboxAddress(user.FirstName, user.Email));
                message.Subject = "Weekly Newsletter";
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<p> On " + DateTime.Now.AddDays(2).ToLongDateString() +
                            $" Hello {user.FirstName}! <br/>" + "\"Your subscription will expire in two days. " +
                            "Continue to enjoy reading our news by subscribing again </p> \";!"
                };

                using (var emailClient = new SmtpClient())
                {
                    emailClient.Connect(_configuration["EmailConfiguration:SmtpServer"], int.Parse(_configuration["EmailConfiguration:SmtpPort"]), true);
                    emailClient.Authenticate(_configuration["EmailConfiguration:SmtpUsername"], _configuration["EmailConfiguration:SmtpPassword"]);
                    emailClient.Send(message);
                    emailClient.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log or rethrow if necessary)
                _logger.LogError($"An error occurred: {ex.Message}");
                _logger.LogError($"Message Faile: {ex.Message}");
            }
        }
    }

    public class User
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
