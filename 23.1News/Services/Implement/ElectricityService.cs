
using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Azure;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;
using System.Net.Http;


namespace _23._1News.Services.Implement
{
    public class ElectricityService : IElectricityService
    {
        private readonly HttpClient _httpClient;

        public ElectricityService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("electricityPrice");
        }

        //public List<string> GetCities()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Electricity> GetElectricityPrice()
        {
            var electricityResponse = await _httpClient.GetStringAsync("");

            return JsonConvert.DeserializeObject<Electricity>(electricityResponse);

        }

    }
}

