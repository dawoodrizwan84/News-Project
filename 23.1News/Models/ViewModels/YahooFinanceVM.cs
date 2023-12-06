using _23._1News.Models.Db;

namespace _23._1News.Models.ViewModels
{
    public class YahooFinanceVM
    {
        public List<YahooFinance.Quote> Quotes { get; set; }
        public List<YahooFinance.News> News { get; set; }
    }
}
