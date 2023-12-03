using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using System.Net.Http;

namespace _23._1News.Services.Implement
{
    public class ExchangeRatesServicve : IExchangeRatesService
    {
        
        private HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;

        public ExchangeRatesServicve(IConfiguration configuration,
             IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
           
            _httpClient = httpClientFactory.CreateClient("dailyPrices");
        }

        //private HttpClient _httpClient;

        //public ExchangeRatesServicve(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}


        public async Task<ExchangeRates> GetRateAsync() 
        {
            var response = await _httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");
                   
            return JsonConvert.DeserializeObject<ExchangeRates>(response);
        }
    }
}
