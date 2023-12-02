
using Newtonsoft.Json;
using System.Globalization;

namespace _23._1News.Models.Db
{
    public class SpotData
    {
        public DateTime DateAndTime { get; set; }
        public string AreaName { get; set; }
        public string Price { get; set; }
        // New property for the decimal representation of Price
        public decimal DecimalPrice
        {
            get
            {
                Price = Price.Replace(" ", "").Replace(',', '.');
                CultureInfo culture = CultureInfo.InvariantCulture;

                if (decimal.TryParse(Price, NumberStyles.Number, culture, out decimal result))
                {
                    return result/1000;
                }
                else
                {
                    return 0;
                }
            }
        }
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

//public class TodaysSpotPrices
//    {
//        public List<SpotData> SpotData { get; set; }
//    }

//    public class Electricity
//    {
//        public List<TodaysSpotPrices> TodaysSpotPrices { get; set; }
//    }



//}