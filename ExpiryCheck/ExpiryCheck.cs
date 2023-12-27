using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using _23._1News.Models.Db;
using _23._1News.Data;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Queues;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ExpiryCheck
{
    public class ExpiryCheck
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;

        private readonly QueueClient _queueClient;

        public ExpiryCheck(ILoggerFactory loggerFactory, IConfiguration configuration,
            ApplicationDbContext applicationDbContext)
        {
            _logger = loggerFactory.CreateLogger<ExpiryCheck>();
            _configuration = configuration;
            _applicationDbContext = applicationDbContext;

            var connectionString = _configuration["AzureWebJobsStorage"];
            var queueString = "expirequeue";

            _queueClient = new QueueClient(connectionString, queueString,
                new QueueClientOptions
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                });
        }

        [Function("ExpiryCheck")]
        public void Run([TimerTrigger("0 00 12 * * 5")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            List<Subscription> ActiveSubscriptionsExpires = _applicationDbContext.Subscriptions
              .Where(s => s.IsActive && s.Created.AddDays(30) >= DateTime.UtcNow && s.Created.AddDays(28) <= DateTime.UtcNow)
              .ToList();

            foreach (var user in ActiveSubscriptionsExpires)
            {
                try
                {
                    _queueClient.CreateIfNotExists();
                    _queueClient.SendMessage(JsonConvert.SerializeObject(user, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Message could not be sent to queue (Error: {ex.Message})");
                }
            }

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
