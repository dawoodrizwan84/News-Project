using _23._1News.Models.Email;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace _23._1News.Helpers
{
    public class EmailHelper:IEmailHelper
    {
        private readonly IEmailConfiguration _emailConfiguration;
        
        public EmailHelper(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public void SendEmail(EmailMessage emailMessage) 
        { 
            var message=new MimeMessage();
            message.Sender = MailboxAddress.Parse(emailMessage.FromAddress.Address);
            message.Sender.Name = emailMessage.FromAddress.Name;
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name,x.Address)));
            message.From.Add(message.Sender);
            message.Subject = emailMessage.Subject;
            message.Body=new TextPart(TextFormat.Html) 
            { Text=emailMessage.Content};

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.SmtpUsername,_emailConfiguration.SmtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
        }


    //public Task SendEmailAsync(EmailMessage emailMessage)
    //{
    //    var message = new MimeMessage();
    //    message.Sender = MailboxAddress.Parse(emailMessage.FromAddress.Address);
    //    message.Sender.Name = emailMessage.FromAddress.Name;
    //    message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
    //    message.From.Add(message.Sender);
    //    message.Subject = emailMessage.Subject;
    //    message.Body = new TextPart(TextFormat.Html)
    //    { Text = emailMessage.Content };

    //    using (var emailClient = new SmtpClient())
    //    {
    //        emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
    //        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
    //        emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
    //        emailClient.Send(message);
    //        emailClient.Disconnect(true);
    //    }
    //}

}
}
