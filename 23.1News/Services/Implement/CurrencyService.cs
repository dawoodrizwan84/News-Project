using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Azure.Data.Tables;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.Http;

namespace _23._1News.Services.Implement
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _spotHttpClient = new HttpClient();
        TableServiceClient _tableServiceClient;
        private readonly HttpClient _httpClient;
        public CurrencyService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _tableServiceClient = new TableServiceClient(_configuration["AzureWebJobsStorage"]);
            _httpClient = httpClientFactory.CreateClient("exchangePrice");
        }

        public async Task<SpotRate> GetRate() 
        {
            var httpClient = new HttpClient();
            //var exchangeResponse = await _httpClient.GetStringAsync("");
            var response = await httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");
            return JsonConvert.DeserializeObject<SpotRate>(response);

        }

        public async Task<string> GetSpotRateAsync()
        {
            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Get,
            //    RequestUri = new Uri("https://currency-exchange.p.rapidapi.com/exchange?from=USD&to=SEK"),
            //    Headers =
            //    {
            //        { "X-RapidAPI-Key", _configuration["RapidApiKey"] },
            //        { "X-RapidAPI-Host", "currency-exchange.p.rapidapi.com" },
            //    },
            //};

            HttpRequestMessage requestMessage = new(HttpMethod.Get, "https://api.exchangerate-api.com/v4/latest/USD");
            
            //requestMessage.Headers.Add("X-RapidAPI-Key", _configuration["RapidApiKey"]);
            //requestMessage.Headers.Add("X-RapidAPI-Host", _configuration["RapidApiHost"]);

            using (var response = await _spotHttpClient.SendAsync(requestMessage))
            {
                //response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);

                return body;
            }

            
        }

        //public void SaveData(TodaysSpotRate todaysSpotRate)
        //{
        //    var goroupedSpotRate = todaysSpotRate.SpotRates
        //                       .SelectMany(s => s.ExchangeRate)
        //                       .GroupBy(a => a.)
        //                       .Select(d => new CurrencyHighLowEntity()
        //                       {
        //                           PartitionKey = d.Key,
        //                           Timestamp = DateTime.Now.Date,
        //                           HighestPrice = d.Max(m => Convert.ToDecimal(m.ExchangeRate) / 1000),
        //                           LowestProice = d.Min(m => Convert.ToDecimal(m.ExchangeRate) / 1000),
        //                           HourHighest = d.MaxBy(m => Convert.ToDecimal(m.ExchangeRate) / 1000).DateAndTime.Hour,
        //                           HourLowest = d.MinBy(m => Convert.ToDecimal(m.ExchangeRate) / 1000).DateAndTime.Hour

        //                       })
        //                       .ToList();

        //    //_db.SpotHighAndLows.AddRange(goroupedSpotRate); ==>> To save in datababase.
        //    //_db.SaveChanges();

        //    TableClient tableClient = _tableServiceClient.GetTableClient
        //                                (tableName: "currencyhighowtable");
        //    tableClient.CreateIfNotExists();
        //    Random rnd = new Random();

        //    foreach (var item in goroupedSpotRate)
        //    {
        //        item.RowKey = item.PartitionKey + item.Timestamp /*+ rnd.Next(10000).ToString()*/;
        //        tableClient.AddEntityAsync<CurrencyHighLowEntity>(item);
        //        var tableRows = tableClient.Query<CurrencyHighLowEntity>().ToList();
        //    }
           
        //}
    }
}

