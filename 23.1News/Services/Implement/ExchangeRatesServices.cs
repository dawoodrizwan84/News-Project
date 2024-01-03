using _23._1News.Models.Db;
using _23._1News.Services.Abstract;

using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

using Azure.Data.Tables;
using Microsoft.AspNetCore.Components;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents;
using Microsoft.CodeAnalysis;

using Microsoft.WindowsAzure.Storage;



using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _23._1News.Services.Implement
{
    public class ExchangeRatesServices : IExchangeRatesService
    {
        private readonly IConfiguration _configuration;
        private readonly TableClient _tableClient;
        private readonly HttpClient _httpClient;

        public ExchangeRatesServices(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;

            var connectionString = _configuration["AzureWebJobsStorage"];
            //var storageAccount = StorageAccount.Parse(connectionString);
            _tableClient = new TableClient(connectionString, "exchangeprices");
            _httpClient = httpClientFactory.CreateClient("exchangeprices");

        }

        public async Task<ExchangeRates> GetRateAsync()
        {
            var response = await _httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");

            return JsonConvert.DeserializeObject<ExchangeRates>(response);
        }


        public async Task<IEnumerable<ExchangeRateEntity>> HistoricalData(Dictionary<string, decimal> exchangeRates, DateTime date)
        {
            

            await _tableClient.CreateIfNotExistsAsync();

            var result = _tableClient.Query<ExchangeRateEntity>(filter: "Timestamp ge datetime'2024-01-01T00:00:00.000Z' and Timestamp le datetime'2024-01-01T23:59:59.000Z'");
            return result;

          
        }
    }
    }




