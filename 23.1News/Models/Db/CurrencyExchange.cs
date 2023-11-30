namespace _23._1News.Models.Db
{
    public class SpotRate
    {
        public DateTime Date { get; set; } = DateTime.Now;
        //public string ExchangeRate { get; set; }
        //public string Currency { get; set; }

        public string Base { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }

       

    }

    public class TodaysSpotRate
    {
        public List<SpotRate> SpotRates { get; set; }

        //public TodaysSpotRate()
        //{
        //    TodaysRate = new List<CurrencyExchange>();
        //}

    }

    public class CurrencyExchange
    {
        public List<TodaysSpotRate> TodaySpotRate { get; set; }
    }
}