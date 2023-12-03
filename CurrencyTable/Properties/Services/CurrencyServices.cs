using _23._1News.Models.Db;
using Azure.Data.Tables;
using CurrencyTable.Properties.Model;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTable.Properties.Services
{
    public class CurrencyServices : ICurrencyServices
    {
        private readonly IConfiguration _configuration;
        private HttpClient _newHttpClient = new HttpClient();
        TableServiceClient _tableServiceClient;
        private readonly HttpClient _httpClient;


        public CurrencyServices(IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _tableServiceClient = new TableServiceClient(_configuration["AzureWebJobsStorage"]);
            _httpClient = httpClientFactory.CreateClient("exchangeprices");

        }

        public async Task<string> GetRateAsync()
        {
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, "https://api.exchangerate-api.com/v4/latest/USD");

            using (var response = await _httpClient.SendAsync(httpRequestMessage))
            {
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return body;
            }

        }

        public async Task SaveDataAsync(TodaysRate todaysRate)
        {

            var queryRateEntities = todaysRate.currencyExchangeRates
                                 .SelectMany(s => s.Rates)
                                .Take(10)
                                .Select((rate, index) => new TodaysRateEntity()
                                {
                                    PartitionKey = rate.Key,
                                    RowKey = $"{rate.Key}_{index}",
                                    Rate1 = rate.Value,
                                    Timestamp = DateTime.UtcNow
                                })
                                .ToList();

            TableClient tableClient = _tableServiceClient.GetTableClient(tableName: "exchangeprices");
            try
            {
                await _tableServiceClient.CreateTableIfNotExistsAsync("exchangeprices");

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");
            }

            Random rnd = new Random();


            foreach (var item in queryRateEntities)
            {
                //item.RowKey = item.PartitionKey + item.Timestamp;
                tableClient.AddEntityAsync<TodaysRateEntity>(item);


            }
            var tableRows = tableClient.Query<TodaysRateEntity>().ToList();

        }
    }
}
