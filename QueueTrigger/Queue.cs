using System;
using _23._1News.Models.Db;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace QueueTrigger
{
    public class Queue
    {
        private readonly ILogger _logger;

        public Queue(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Queue>();
        }

        [Function("Queue")]
        public void Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] User message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.Email}");
        }
    }
}
