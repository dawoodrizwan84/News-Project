
using Newtonsoft.Json;

namespace _23._1News.Models.Db
{
    public class WeatherForecast
    {

        [JsonProperty("summary")]
        public string Summary { get; set; } = string.Empty;

        [JsonProperty("city")]
        public string City { get; set; } = string.Empty;

        [JsonProperty("lang")]
        public string Language { get; set; } = string.Empty;

        [JsonProperty("temperatureC")]
        public int TemperatureCelsius { get; set; }

        [JsonProperty("temperatureF")]
        public int TemperatureFahrenheit { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        [JsonProperty("windSpeed")]
        public int WindSpeed { get; set; }

        [JsonProperty("unixTime")]
        public int UnixTime { get; set; }

        [JsonProperty("date")]
        public DateTime DateAndTime { get; set; }

        [JsonProperty("icon")]
        public WeatherForecastIcon Icon { get; set; }
    }

    public class WeatherForecastIcon
    {
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;
    }
}



