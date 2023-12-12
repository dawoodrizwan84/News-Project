// The 'From' and 'To' fields are automatically populated with the values specified by the binding settings.
//
// You can also optionally configure the default From/To addresses globally via host.config, e.g.:
//
// {
//   "sendGrid": {
//      "to": "user@host.com",
//      "from": "Azure Functions <samples@functions.com>"
//   }
// }
using Microsoft.Azure.WebJobs;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
//using User = _23._1News.Models.Db.User;
using System.Net.Mail;

//namespace SendGridNewsLetter
//{
//    public class SendEmail
//    {
//        [FunctionName("SendEmail")]
//        [return: SendGrid(ApiKey = "23NewsKey", To = "", From = "send.23.1news@outlook.com")]
//        public SendGridMessage Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] User user, ILogger log)
//        {
//            log.LogInformation($"C# Queue trigger function processed order: {user.Email}");

//            SendGridMessage message = new SendGridMessage()
//            {
//                Subject = $"Weekly Newsletter From 23.1News."
//            };

//            message.AddTo(user.Email);

//message.AddContent("text/html", $"<h3>Good afernoon {user.FirstName}!</h3>" +
//    $"<h4>Articles of your choice.</h4>");
//return message;


//        }
//    }
//public class Order
//{
//    public string OrderId { get; set; }
//    public string CustomerName { get; set; }
//    public string CustomerEmail { get; set; }
//}
//}
