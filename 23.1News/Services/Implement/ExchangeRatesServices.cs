using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Components;
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


        public async Task<List<ExchangeHistoricalEntity>> GetAllHistoricalData(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = new List<ExchangeHistoricalEntity>();
                var table = _tableServiceClient.GetTableClient("exchangeprices");

                var days = DateTime.UtcNow.AddDays(-7);

                await foreach (var item in table.QueryAsync<ExchangeHistoricalEntity>())
                {
                    if (item.Timestamp.HasValue && item.Timestamp.Value.UtcDateTime >= startDate && item.Timestamp.Value.UtcDateTime <= days)
                    {
                        result.Add(item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the retrieve operation.
                throw ex;
            }
        }

    }
}



