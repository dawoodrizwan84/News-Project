using _23._1News.Models.Email;

namespace _23._1News.Helpers
{
    public interface IEmailHelper
    {
        void SendEmail(EmailMessage message);
    }
}
