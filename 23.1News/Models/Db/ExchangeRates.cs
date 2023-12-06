namespace _23._1News.Models.Db
{
    public class ExchangeRates
    {
        public DateTime DateAndTIme { get; set; } = DateTime.Now;

        public string Base { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }

    }

    public class TodaysRate
    {
        public List<ExchangeRates> ExchangeRates { get; set; }

      
    }

    public class CurrencyPrice
    {
        public List<TodaysRate> TodaysRate { get; set;}
    }
}
