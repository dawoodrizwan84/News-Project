using System;
using System.Data.Entity;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Queue_NoReference.Data;
using Queue_NoReference.Model;
using Queue_NoReference.Services;

namespace Queue_NoReference
{
    public class FunctionMessage
    {
        private readonly ILogger _logger;
        private readonly IUserQServices _userQServices;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;

        public FunctionMessage(ILoggerFactory loggerFactory, IUserQServices userQServices,
                 IConfiguration configuration, AppDbContext appDbContext)
        {
            _logger = loggerFactory.CreateLogger<FunctionMessage>();
            _userQServices = userQServices;
            _configuration = configuration;
            _appDbContext = appDbContext;
            
        }

        [Function("FunctionMessage")]
        public void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var connectionString = _configuration["AzureWebJobsStorage"];
            var queueString = "newsletterqueue";


            QueueClient queueClient = new QueueClient(connectionString, queueString,
                        new QueueClientOptions
                        {
                            MessageEncoding = QueueMessageEncoding.Base64
                        });

            List<UserQ> NewsLetterUsers = _appDbContext.userQ.ToList();
            //.Include(x => x.UserCategories).ToList();

            foreach (var item in NewsLetterUsers)
            {
                queueClient.CreateIfNotExists();

                try
                {
                    UserQ queueUser = new UserQ();

                    queueUser.FirstName = item.FirstName;
                    queueUser.LastName = item.LastName;
                    queueUser.Email = item.Email;
                    queueUser.UserCategories = _appDbContext.categoryQ
                            .Where(cate => cate.CategoryUsers.Any(user => user.Id == user.Id))
                    .ToList();


                    queueClient.SendMessage(JsonConvert.SerializeObject(item, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
                }
                catch (Exception ex)
                {

                    _logger.LogInformation($"Message could not be sent to queue (Error: {ex.Message})");
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
