using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace _23._1News.Services.Implement
{
    public class ExchangeRatesServices : IExchangeRatesService
    {
        private readonly Microsoft.WindowsAzure.Storage.Table.CloudTable _table;
        private HttpClient _httpClient = new HttpClient();
        private readonly IConfiguration _configuration;

        public ExchangeRatesServices(IConfiguration configuration,
             IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("dailyPrices");

            //Initialize the CouldTable with connection details
            var cloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.Parse(_configuration["AzureWebJobsStorage"]);
            var tableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

            //_table = tableClient.GetTableReference("exchangeprices");
            _table.CreateIfNotExistsAsync();

        }

        public async Task<ExchangeRates> GetRateAsync()
        {
            var response = await _httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");


            return JsonConvert.DeserializeObject<ExchangeRates>(response);
        }




    }
}
