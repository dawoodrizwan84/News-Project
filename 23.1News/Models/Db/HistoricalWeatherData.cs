using Newtonsoft.Json;

namespace _23._1News.Models.Db
{
    public class HistoricalWeatherData
    {
        public int Id { get; set; }
        [JsonProperty("summary")]
        public string Summary { get; set; } = string.Empty;

        [JsonProperty("city")]
        public string City { get; set; } = string.Empty;
        [JsonProperty("temperatureC")]
        public int TemperatureCelsius { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("windSpeed")]
        public int WindSpeed { get; set; }

        [JsonProperty("date")]
        public DateTime DateAndTime { get; set; }

    }
}
