using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

    public class UserService
    {
        //private readonly IConfiguration _configuration;
        //private readonly ILogger<UserService> _logger;

        //public UserService(IConfiguration configuration, ILogger<UserService> logger)
        //{
        //    _configuration = configuration;
        //    _logger = logger;
        //}

        //public void ProcessQueueMessage([QueueTrigger("newsletterqueue")] string userJson)
        //{
        //    try
        //    {
        //        // Deserialize the JSON message from the queue
        //        var user = JsonConvert.DeserializeObject<User>(userJson);

        //        // Call the SendEmail method
        //        SendEmail(user);

        //        _logger.LogInformation($"Email sent successfully to {user.Email}.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"An error occurred: {ex.Message}");
        //        // Optionally, you can rethrow the exception or handle it as needed.
        //    }
        //}

        //public void SendEmail(User user)
        //{
        //    try
        //    {
        //        var message = new MimeMessage();
        //        message.Sender = MailboxAddress.Parse(_configuration["EmailConfiguration:FromAddress"]); // Set the FromAddress from configuration
        //        message.Sender.Name = "Your Sender Name"; // Set your sender name

        //        message.To.Add(MailboxAddress.Parse(user.Email)); // Send email to the user's email address
        //        message.Subject = "Your Subject"; // Set the subject of the email

        //        message.Body = new TextPart(TextFormat.Html)
        //        {
        //            Text = $"Hello {user.FirstName} {user.LastName},<br/><br/> " // Use the email content from the user object
        //        };

        //        using (var emailClient = new SmtpClient())
        //        {
        //            emailClient.Connect(_configuration["EmailConfiguration:SmtpServer"], int.Parse(_configuration["EmailConfiguration:SmtpPort"]), true);
        //            emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
        //            emailClient.Authenticate(_configuration["EmailConfiguration:SmtpUsername"], _configuration["EmailConfiguration:SmtpPassword"]);
        //            emailClient.Send(message);
        //            emailClient.Disconnect(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (log or rethrow if necessary)
        //        _logger.LogError($"An error occurred: {ex.Message}");
        //    }
        //}
    }

