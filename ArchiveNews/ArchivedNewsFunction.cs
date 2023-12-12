using System;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using _23._1News.Services.Implement;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;


namespace ArchiveNews
{
    public class ArchivedNewsFunction
    {
        private readonly ILogger _logger;
        private readonly IArticleService _articleService;




        public ArchivedNewsFunction(ILoggerFactory loggerFactory,
                                    IArticleService articleService)
        {
            _logger = loggerFactory.CreateLogger<ArchivedNewsFunction>();
            _articleService = articleService;
        }

        [Function("ArchivedNewsFunction")]
        public void Run([TimerTrigger("0 */5 * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");


            var articlesToArchive = _articleService.GetArchiveNews();



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
