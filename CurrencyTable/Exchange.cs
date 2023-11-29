using System;
using _23._1News.Services.Abstract;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CurrencyTable
{
    public class Exchange
    {
        private readonly ILogger _logger;
        private readonly ICurrencyExchange _currencyExchange;
        public Exchange(ILoggerFactory loggerFactory, ICurrencyExchange currencyExchange)
        {
            _logger = loggerFactory.CreateLogger<Exchange>();
            _currencyExchange = currencyExchange;
        }

        [Function("Exchange")]
        public void Run([TimerTrigger("0 14 * * * *", RunOnStartup = true)] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            var spotRates = _currencyExchange.GetSpotRateAsync().Result;
            
            if (myTimer.ScheduleStatus is not null) 
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
