using Azure;
using Azure.Data.Tables;
using Microsoft.Azure.Documents;
using System.Collections.Concurrent;
using System.Runtime.Serialization;


namespace _23._1News.Models.Db
{

    public class ExchangeRates
    {

        public DateTime DateAndTime { get; set; } = DateTime.Now;

        public string Base { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }


    }


    public class TodaysRate
    {
        public List<ExchangeRates> ExchangeRates { get; set; }

    }

    public class CurrencyPrice
    {
        public List<TodaysRate> todaysRate { get; set; }
    }



}
