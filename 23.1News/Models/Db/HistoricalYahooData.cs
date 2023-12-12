using Newtonsoft.Json;

namespace _23._1News.Models.Db
{
    public class HistoricalYahooData
    {

        public string Symbol { get; set; }
        public string ShortName { get; set; }
        public float Score { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }

    }

}

