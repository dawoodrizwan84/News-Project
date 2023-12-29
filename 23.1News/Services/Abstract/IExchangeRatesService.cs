using _23._1News.Models.Db;

namespace _23._1News.Services.Abstract
{
    public interface IExchangeRatesService
    {
        Task<ExchangeRates> GetRateAsync();
       void SaveExchangeRateData(Dictionary<string, decimal> exchangeRates, DateTime date);
    }
}
