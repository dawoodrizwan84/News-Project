using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using HighestLowestElectricityPrice.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;



namespace HighestLowestElectricityPrice.Services
{
    public class SpotService : ISpotService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _spotHttpClient = new HttpClient();
        TableServiceClient _tableServiceClient;

        public SpotService(IConfiguration configuration)
        {
            _configuration = configuration;
            _tableServiceClient = new TableServiceClient(_configuration["AzureWebJobsStorage"]);
            

        }

        public async Task<TodaysSpotData> GetSpotMetrics()
        {
            var spotPricesRequest = await _spotHttpClient.GetStringAsync(_configuration["MyElectricityPriceAPIAddress"]);

            // Use JsonSerializer.Deserialize instead of JsonConverter.DeserializeObject
            var spotPriceMetrics = JsonConvert.DeserializeObject<TodaysSpotData>(spotPricesRequest);

            return spotPriceMetrics;
        }
        public void SaveSportData(TodaysSpotData todayData)
        {
            var groupedSpotData = todayData.TodaysSpotPrices
                .SelectMany(s => s.SpotData)
                .GroupBy(a => a.AreaName)
                .Select(d => new SpotHighLowEntity()
                {
                    PartitionKey = d.Key,
                    Timestamp = DateTime.Now.Date,
                    HighestPrice = d.Max(a => Convert.ToDecimal(a.Price)/1000),
                    LowestPrice = d.Min(a => Convert.ToDecimal(a.Price)/1000),
                    HourHighest = d.MaxBy(d => Convert.ToDecimal(d.Price)/1000).DateTime.Hour,
                    HourLowest = d.MinBy(d => Convert.ToDecimal(d.Price)/1000).DateTime.Hour,
                })
                .ToList();

            TableClient tableClient = _tableServiceClient.GetTableClient
            (tableName: "spothighlowtable");
            tableClient.CreateIfNotExistsAsync();
            Random rnd = new();
            foreach (var item in groupedSpotData) 
            {
                item.RowKey = item.PartitionKey+item.Timestamp+rnd.Next(10000).ToString();
                tableClient.AddEntityAsync<SpotHighLowEntity>(item);
                var tableRows= tableClient.Query<SpotHighLowEntity>().ToList();
            };
        }
    }
}
