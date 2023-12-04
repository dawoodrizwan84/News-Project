
﻿using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Azure;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using System.Net.Http;


namespace _23._1News.Services.Implement
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _db;

        public WeatherService(IHttpClientFactory httpClientFactory, ApplicationDbContext db)
        {
            _httpClient = httpClientFactory.CreateClient("weatherForecast");
            _db = db;
        }

       
        public async Task<WeatherForecast> GetWeatherForecast(string city)
        {
            var forecastResponse = await _httpClient.GetStringAsync($"forecast?city={city}& lang= en");

            return JsonConvert.DeserializeObject<WeatherForecast>(forecastResponse);
        }
        public void StoreHistoricalWeather(WeatherForecast weatherForecast)
        {

            HistoricalWeatherData historicalWeather = ConvertToHistoricalWeatherData(weatherForecast);
               // _db.HistoricalWeatherDatas.Add(historicalWeather);
            _db.SaveChanges();
        }

        public  HistoricalWeatherData ConvertToHistoricalWeatherData(WeatherForecast weatherForecast)
        {
            return new HistoricalWeatherData
            {
                Summary = weatherForecast.Summary,
                City = weatherForecast.City,
                TemperatureCelsius = weatherForecast.TemperatureCelsius,
                Humidity = weatherForecast.Humidity,
                WindSpeed = weatherForecast.WindSpeed,
                DateAndTime = weatherForecast.DateAndTime,
                
            };
        }
    }
}

