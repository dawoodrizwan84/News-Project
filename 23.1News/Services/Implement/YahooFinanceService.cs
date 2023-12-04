using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace _23._1News.Services.Implement
{
    public class YahooFinanceService : IYahooFinanceService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _db;

        public YahooFinanceService(IHttpClientFactory httpClientFactory, ApplicationDbContext db)
        {
            _httpClient = httpClientFactory.CreateClient("YahooFinance");
            _db = db;
        }

        //public async Task<List<YahooFinance.Quote>> GetFinancialDataAsync(string symbol)
        //{
        //    // Example API endpoint for financial data
        //    string apiUrl = $"https://apidojo-yahoo-finance-v1.p.rapidapi.com/auto-complete?q={symbol}&region=US";

        //    var financeResponse = await _httpClient.GetStringAsync(apiUrl);

        //    // Deserialize JSON response
        //    var result = JsonConvert.DeserializeObject<YahooFinance.Rootobject>(financeResponse);

        //    return result?.quotes != null ? new List<YahooFinance.Quote>(result.quotes) : null;
        //}

        public async Task SaveFinancialDataAsync(string symbol)
        {
            try
            {
                List<YahooFinance.Quote> financialData = await GetFinancialDataAsync(symbol);

                if (financialData != null)
                {
                    _db.HistoricalYahooData.AddRange(financialData.Select(q => new HistoricalYahooData
                    {
                        Symbol = q.symbol,
                        ShortName = q.shortname,
                        Score = q.score,
                        
                    }));

                    await _db.SaveChangesAsync();
                }
            }
            catch (DbUpdateException dbEx)
            {
                
                Console.WriteLine($"Database update error: {dbEx.Message}");

               
                throw new Exception("Error saving financial data to the database. Please try again later.");
            }
            catch (HttpRequestException httpEx)
            {
               
                Console.WriteLine($"HTTP request error: {httpEx.Message}");

                
                throw new Exception("Error fetching financial data. Please try again later.");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Unexpected error: {ex.Message}");

                
                throw;
            }
        }


        public async Task<List<YahooFinance.Quote>> GetFinancialDataAsync(string symbol)
        {
            string apiUrl = $"/auto-complete?q={symbol}&region=US";

            var financeResponse = await _httpClient.GetStringAsync(apiUrl);

            var result = JsonConvert.DeserializeObject<YahooFinance.Rootobject>(financeResponse);

            return result?.quotes?.ToList() ?? new();
        }


        public async Task<List<YahooFinance.News>> GetNewsAsync(string symbol)
        {
            
            string apiUrl = $"/auto-complete?q=tesla&region=US{symbol}/news";

            var response = await _httpClient.GetStringAsync(apiUrl);

            var result = JsonConvert.DeserializeObject<YahooFinance.Rootobject>(response);

            return result?.news?.ToList() ?? new();
        }

        public async Task<List<HistoricalYahooData>> GetHistoricalDataAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            
            string apiUrl = $"/auto-complete?q=tesla&region=US{symbol}/historical?start={startDate:yyyy-MM-dd}&end={endDate:yyyy-MM-dd}";

            var response = await _httpClient.GetStringAsync(apiUrl);

            var result = JsonConvert.DeserializeObject<List<HistoricalYahooData>>(response);

            return result ?? new();
        }

       
        
    }
}

