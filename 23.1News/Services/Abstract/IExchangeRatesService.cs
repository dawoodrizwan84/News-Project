using _23._1News.Models.Db;

namespace _23._1News.Services.Abstract
{
    public interface IExchangeRatesService
    {
        Task<ExchangeRates> GetRateAsync();
        Task<IEnumerable<ExchangeRateEntity>> HistoricalData(Dictionary<string, decimal> exchangeRates, DateTime date);
    }
}
