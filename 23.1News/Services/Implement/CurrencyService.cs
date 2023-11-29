using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Newtonsoft.Json;

namespace _23._1News.Services.Implement
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _spotHttpClient = new HttpClient();
        //TableServiceClient _tableServiceClient;

        public CurrencyService(IConfiguration configuration)
        {
            _configuration = configuration;
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

            HttpRequestMessage requestMessage = new(HttpMethod.Get, "https://currency-exchange.p.rapidapi.com/exchange?from=USD&to=SEK&q=2");
            requestMessage.Headers.Add("X-RapidAPI-Key", _configuration["RapidApiKey"]);
            requestMessage.Headers.Add("X-RapidAPI-Host", _configuration["RapidApiHost"]);

            using (var response = await _spotHttpClient.SendAsync(requestMessage))
            {
                //response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);

                return body;
            }

        }
    }
}

