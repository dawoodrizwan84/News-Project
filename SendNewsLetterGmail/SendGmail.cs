using System;
using System.Collections.Generic;
using System.IO;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public SendGmail(ILogger<SendGmail> logger, IConfiguration configuration
            )
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

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $"<img src=\"https://newsprojectstorage.blob.core.windows.net/newsimages/logo.png\" " +
                    $"style=\"height: 40px; width: 150px;\"> </br></br>" +
                    $"<p> On {DateTime.Now.AddDays(5).ToLongDateString()} Hello {user.FirstName}! <br/>";

                // Create a string to hold the concatenated categories
                var categoriesString = "";

                if (user.UserCategories != null)
                {
                    foreach (var item in user.UserCategories)
                    {
                        categoriesString += $"{item.Name}, ";
                    }

                    // Remove the trailing comma and space if there are categories
                    if (!string.IsNullOrEmpty(categoriesString))
                    {
                        categoriesString = categoriesString.TrimEnd(',', ' ');
                    }

                    // Add the categories to the HTML body
                    bodyBuilder.HtmlBody += $"Your Weekly Newsletter of Choice: <strong>{categoriesString}</strong> <br/>";

                    // Continue with the rest of your HTML body
                    bodyBuilder.HtmlBody += "Thank you for subscribing. </br></br>" +
                        "Please visit 23.1 News to read the latest news: <a href=\"https://231news20231115124158.azurewebsites.net/\">Link to News</a></p>";

                    message.Body = bodyBuilder.ToMessageBody();

                    using (var emailClient = new SmtpClient())
                    {
                        emailClient.Connect(_configuration["SmtpServer"], int.Parse(_configuration["SmtpPort"]), SecureSocketOptions.StartTls);
                        emailClient.Authenticate(_configuration["EmailAddress"], _configuration["SmtpPassword"]);
                        emailClient.Send(message);
                        emailClient.Disconnect(true);
                    }
                }
               
            }
            catch (Exception ex)
            {
                // Handle exceptions (log or rethrow if necessary)
                _logger.LogError($"An error occurred: {ex.Message}");
            }


        }

        //public class User
        //{
        //    public string FirstName { get; set; } = string.Empty;
        //    public string LastName { get; set; } = string.Empty;
        //    public string Email { get; set; } = string.Empty;

        //    public virtual ICollection<Category> UserCategories { get; set; } = new List<Category>();
        //}
    }
}