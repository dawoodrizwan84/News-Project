using Azure.Data.Tables;
using CurrencyTable.Model;
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

namespace CurrencyTable.Services
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

            var top10Currencies = new List<string> { "KWD", "BHD", "OMR", "JOD", "GIP", "GBP", "KYD", "CHF", "EUR", "USD" };

            var top10Rates = new Dictionary<string, decimal>();

            foreach (var currency in top10Currencies)
            {

                if (todaysRates.Rates.ContainsKey(currency))
                {

                    top10Rates.Add(currency, todaysRates.Rates[currency]);
                }


            }

            // Create the table client
            TableClient tableClient = _tableServiceClient.GetTableClient(tableName: "exchangeprices");
            try
            {
                await _tableServiceClient.CreateTableIfNotExistsAsync("exchangeprices");

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");
            }
                        
            foreach (var (currency, rate) in top10Rates)
            {
                var entity = new TodaysRateEntity()
                {
                    Currency = currency,
                    Rate = rate,
                    Timestamp = DateTime.UtcNow,
                    RowKey = $"{currency}_{DateTime.UtcNow.Ticks}",
                    PartitionKey = currency
                };

                try
                {
                    await tableClient.AddEntityAsync(entity);

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error adding entity: {ex.Message}");
                }
            }
        }


    }


}

