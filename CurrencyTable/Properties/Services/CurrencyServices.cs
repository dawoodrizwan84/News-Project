using _23._1News.Models.Db;
using Azure.Data.Tables;
using CurrencyTable.Properties.Model;
using Google.Protobuf.WellKnownTypes;
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

        public async Task SaveDataAsync(CurrencyRates todaysRates)
        {

            //var queryRateEntities = todaysRates.Rates
            //                     .SelectMany(s => s.Value)
            //                    .Take(10)
            //                    .Select((rate, index) => new TodaysRateEntity()
            //                    {
            //                        PartitionKey = rate.Key,
            //                        RowKey = $"{rate.Key}_{index}",

            //                        Timestamp = DateTime.UtcNow
            //                    })
            //                    .ToList();

            TableClient tableClient = _tableServiceClient.GetTableClient(tableName: "exchangeprices");
            try
            {
                await _tableServiceClient.CreateTableIfNotExistsAsync("exchangeprices");

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");
            }



            foreach (var (symbol, value) in todaysRates.Rates)
            {
                //var rates = todaysRates.Rates
                //            .Select(s => s.Value)
                //            .Distinct() 
                //            .Take(10)
                //            .ToList();
                    
                    
                var newEntity = new TodaysRateEntity()
                {
                    Currency = symbol,
                    Rate = value,
                    Timestamp = DateTime.UtcNow,
                    RowKey = /*DateTime.UtcNow + " " + symbol*/  $"{symbol}_{DateTime.UtcNow.Ticks}",
                    PartitionKey = symbol,


                };

                try
                {
                    await tableClient.AddEntityAsync(newEntity);

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error adding entity: {ex.Message}");
                }


                var tableRows = tableClient.Query<TodaysRateEntity>().ToList();
            }






        }


    }
}
