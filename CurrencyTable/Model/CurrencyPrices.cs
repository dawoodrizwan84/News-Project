namespace CurrencyTable.Model
{
    public class CurrencyPrices
    {
        public List<TodaysRate> TodaysRates { get; set; }
    }

    public class TodaysRate
    {
        public List<CurrencyExchangeRates> currencyExchangeRates { get; set; }
    }

    public class CurrencyExchangeRates
    {
        public DateTime Date { get; set; } = DateTime.Now;

        public string Base { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }
    }
}
