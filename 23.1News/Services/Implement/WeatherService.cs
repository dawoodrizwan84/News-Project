
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

        public WeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("weatherForecast");
        }

        //public List<string> GetCities()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<WeatherForecast> GetWeatherForecast(string city)
        {
            var forecastResponse = await _httpClient.GetStringAsync($"forecast?city={city}& lang= en");

            return JsonConvert.DeserializeObject<WeatherForecast>(forecastResponse);
        }

    }
}

