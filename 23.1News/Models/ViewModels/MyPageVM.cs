namespace _23._1News.Models.ViewModels
{
    public class MyPageVM
    {
        public bool IsEnterpriseUser { get; set; }
        public bool IsSubscribedToNewsletter { get; set; }
        public string? ActiveSubscriptionType { get; internal set; }
    }
}
