using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SendEmail_Gmail
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public static void Run(
        [QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] string message,
        ILogger log)
        {
            log.LogInformation($"Processing queue message: {message}");

            // Extract relevant information from the message and construct your email content
            // For example, you might use JSON to pass details like recipient email, subject, body, etc.

            // Example message format: {"to": "recipient@example.com", "subject": "Hello", "body": "World"}

            // Parse the message JSON
            var emailDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<EmailDetails>(message);

            // Send email
            SendEmail(emailDetails);
        }

        private static void SendEmail(EmailDetails emailDetails)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("senderemailservice23.1@gmail.com", "kqpr jgds jnfz xtsu"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("senderemailservice23.1@gmail.com"),
                Subject = emailDetails.Subject,
                Body = emailDetails.Body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailDetails.To);

            smtpClient.Send(mailMessage);
        }

        private class EmailDetails
        {
            public string To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }
    }
}

