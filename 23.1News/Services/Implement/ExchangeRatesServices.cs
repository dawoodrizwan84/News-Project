using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Components;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.CodeAnalysis;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _23._1News.Services.Implement
{
    public class ExchangeRatesServices : IExchangeRatesService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _newHttpClient = new HttpClient();
        TableServiceClient _tableServiceClient;
        private readonly HttpClient _httpClient;

        public ExchangeRatesServices(IConfiguration configuration,
           IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _tableServiceClient = new TableServiceClient(_configuration["AzureWebJobsStorage"]);
            //_httpClient = httpClientFactory.CreateClient("exchangeprices");

        }

        public async Task<ExchangeRates> GetRateAsync()
        {
            var response = await _httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");

            return JsonConvert.DeserializeObject<ExchangeRates>(response);
        }


        public void SaveExchangeRateData(Dictionary<string, decimal> exchangeRates, DateTime date)
        {
            var connectionString = "AzureWebJobsStorage";
            var tableName = "exchangeprices";

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);

            foreach (var currency in exchangeRates.Keys)
            {
                var entity = new ExchangeRateEntity
                {
                    PartitionKey = currency,
                    RowKey = date.ToString("yyyyMMdd"),
                    Currency = currency,
                    Timestamp = DateTime.UtcNow,

                };

                var insertOrReplaceOperation = Microsoft.Azure.Cosmos.Table.TableOperation.InsertOrReplace((Microsoft.Azure.Cosmos.Table.ITableEntity)entity);
                table.Execute(insertOrReplaceOperation);
            }

        }
    }
}



