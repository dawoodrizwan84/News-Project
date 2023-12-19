using Azure.Storage.Queues;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using TimeTrigger.Data;
using TimeTrigger.Model;
using TimeTrigger.Services;


namespace TimeTrigger
{
    public class FunctionTime
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;


        public FunctionTime(ILoggerFactory loggerFactory,
            IConfiguration configuration, AppDbContext appDbContext
            )
        {
            _logger = loggerFactory.CreateLogger<FunctionTime>();
            _configuration = configuration;
            _appDbContext = appDbContext;

        }

        [Function("FunctionTime")]
        public void Run([TimerTrigger("0 */5 * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var connectionString = _configuration["AzureWebJobsStorage"];
            var queueString = "newsletterqueue";

            QueueClient queueClient = new QueueClient(connectionString, queueString,
                       new QueueClientOptions
                       {
                           MessageEncoding = QueueMessageEncoding.Base64
                       });

            List<UserQ> newsLetterUsers = _appDbContext.userQ.ToList();
                //.Where(u => u.Id == "4a86219b-fd36-4d25-b0b3-634855bb1c38").ToList();

            foreach (var user in newsLetterUsers)
            {
                queueClient.CreateIfNotExists();

                try
                {
                   

                    UserQ queueUser = new UserQ
                    {

                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        UserCategories = user.UserCategories,
                      
                        

                    };

                    queueClient.SendMessage(JsonConvert.SerializeObject(queueUser));

                    _logger.LogInformation($"Message to {user.Email} sent to queue.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Message could not be sent to queue (Error: {ex.Message})");
                }
            }

            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
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
