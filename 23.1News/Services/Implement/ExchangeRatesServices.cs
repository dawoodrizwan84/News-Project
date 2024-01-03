using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Azure.Data.Tables;
using Newtonsoft.Json;

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

            DateTime twentyFourHoursAgo = DateTime.UtcNow.AddHours(-48);
            string formattedTwentyFourHoursAgo = twentyFourHoursAgo.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var result = _tableClient.Query<ExchangeRateEntity>($"Timestamp ge datetime'{formattedTwentyFourHoursAgo}'");

            return result;
        }

    }
}




