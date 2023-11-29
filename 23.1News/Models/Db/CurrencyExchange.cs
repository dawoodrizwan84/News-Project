namespace _23._1News.Models.Db
{
    public class SpotRate
    {
        public DateTime DateAndTime { get; set; } = DateTime.Now;
        public string ExchangeRate { get; set; }
        public string Currency { get; set; }

    }

    public class TodaysSpotRate
    {
        public List<SpotRateList> TodaysRate { get; set; }

        public TodaysSpotRate()
        {
            TodaysRate = new List<SpotRateList>();
        }

    }

    public class SpotRateList
    {
        public List<SpotRate> SpotRates { get; set; }
    }
}