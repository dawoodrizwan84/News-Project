using _23._1News.Models.Db;
using System.Text.Json;

namespace _23._1News.Services.Abstract
{
    public interface IYahooFinanceService
    {
        Task<List<YahooFinance.Quote>> GetFinancialDataAsync(string symbol);
        Task<List<YahooFinance.News>> GetNewsAsync(string symbol);
        Task<List<HistoricalYahooData>> GetHistoricalDataAsync(string symbol, DateTime startDate, DateTime endDate);
    }
}

