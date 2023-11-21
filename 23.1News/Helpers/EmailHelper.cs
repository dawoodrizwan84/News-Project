using _23._1News.Models.Email;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using Humanizer;
using Azure;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace _23._1News.Helpers
{
    public class EmailHelper:IEmailSender,IEmailHelper
    {
        private readonly IEmailConfiguration _emailConfiguration;
        
        public EmailHelper(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }



        public Task SendEmailAsync(string email,string subject,string content)
        {
            string response = "";
            var message = new MimeMessage();
            message.Sender = MailboxAddress.Parse(_emailConfiguration.SenderEmail);
            message.Sender.Name = _emailConfiguration.SenderName;
            message.To.Add(MailboxAddress.Parse(email));
            message.From.Add(message.Sender);
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html)
            { Text = content };

            using (var emailClient = new SmtpClient())
            {
                try 
                {
                    emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
                }

                catch(SmtpCommandException ex) 
                {
                    response = "Error trying to connect:" + ex.Message + "StatusCode:" + ex.StatusCode;
                    return Task.FromResult(response);
                }
                catch (SmtpProtocolException ex)
                {
                    response = "Protocol error while trying  to connect:" + ex.Message;
                    return Task.FromResult(response);
                }

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                
                try
                {
                    emailClient.Send(message);
                }

                catch (SmtpCommandException ex)
                {
                    response = "Error sending message:" + ex.Message + "StatusCode:" + ex.StatusCode;
                    switch(ex.ErrorCode)
                    {
                        case SmtpErrorCode.RecipientNotAccepted:
                            response += "Recipient not accepted:" + ex.Mailbox;
                            break;
                        case SmtpErrorCode.SenderNotAccepted:
                            response += "Sender not accepted:" + ex.Mailbox;
                            Console.WriteLine("\t Sender not accepted:{0}", ex.Mailbox);
                           break;
                        case SmtpErrorCode.MessageNotAccepted:
                            response += "Message not accepted.";
                            break;
                    }
 
                }
                emailClient.Disconnect(true); 
            }
            return Task.CompletedTask;
        }

        public void SendEmail(EmailMessage emailMessage)
        {
            try
            {
                var message = new MimeMessage();
                message.Sender = MailboxAddress.Parse(emailMessage.FromAddress.Address);
                message.Sender.Name = emailMessage.FromAddress.Name;
                message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                message.From.Add(message.Sender);
                message.Subject = emailMessage.Subject;
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = emailMessage.Content
                };

                using (var emailClient = new SmtpClient())
                {
                    emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                    emailClient.Send(message);
                    emailClient.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log or rethrow if necessary)
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
