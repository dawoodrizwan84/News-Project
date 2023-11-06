using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Newtonsoft.Json;

namespace _23._1News.Services.Implement
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("weatherForecast");
        }
        public async Task<WeatherForecast> GetWeatherForecast(string city)
        {
            var WeatherforecastResponse = await _httpClient.GetStringAsync($"forecast?city={city}&lang=en");

            return JsonConvert.DeserializeObject<WeatherForecast>(WeatherforecastResponse);
        }

        //Task<WeatherForecast> IWeatherService.GetWeatherForecast(string city)
        //{
        //    throw new NotImplementedException();
        //}
    }
}