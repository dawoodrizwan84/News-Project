
using Newtonsoft.Json;

namespace _23._1News.Models.Db
{
    public class SpotData
    {
        public DateTime DateAndTime { get; set; }
        public string AreaName { get; set; }
        public string Price { get; set; } 
    }

    public class TodaysSpotPrices
    {
        public List<SpotData> SpotData { get; set; }
    }

    public class Electricity
    {
        public List<TodaysSpotPrices> TodaysSpotPrices { get; set; }
    }



}