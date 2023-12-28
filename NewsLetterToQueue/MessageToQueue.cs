using System;
using _23._1News.Data;
using _23._1News.Models.Db;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NewsLetterToQueue
{
    public class MessageToQueue
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _applicationDbContext;


        public MessageToQueue(ILoggerFactory loggerFactory,
                IConfiguration configuration, ApplicationDbContext applicationDbContext)
        {
            _logger = loggerFactory.CreateLogger<MessageToQueue>();
            _configuration = configuration;
            _applicationDbContext = applicationDbContext;
        }

        [Function("MessageToQueue")]
        public void Run([TimerTrigger("0 00 12 * * 5")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var connectionString = _configuration["AzureWebJobsStorage"];
            var queueString = "newsletterqueue";


            QueueClient queueClient = new QueueClient(connectionString, queueString,
                    new QueueClientOptions
                    {
                        MessageEncoding = QueueMessageEncoding.Base64
                    });


            List<User> NewsLetterUsers = _applicationDbContext.Users
            .Include(user => user.UserCategories)
            .Where(user => user.UserCategories.Count > 0)
            .ToList();

            //.Where(user => user.Id == "4a86219b-fd36-4d25-b0b3-634855bb1c38").ToList();



            foreach (var user in NewsLetterUsers)
            {
                queueClient.CreateIfNotExists();
                try
                {

                    //User queueUser = new User();
                    //queueUser.FirstName = user.FirstName;
                    //queueUser.LastName = user.LastName;
                    //queueUser.Email = user.Email;

                    //user.UserCategories = _applicationDbContext.Categories
                    //    .Where(category => category.CategoryUsers.Any(user => user.Id == user.Id))
                    //     .ToList();


                    //queueUser.SelectedCategoryId = user.SelectedCategoryId;
                    //queueUser.SelectedCategory = _applicationDbContext.Categories
                    //                    .FirstOrDefault(c => c.CategoryId == user.SelectedCategoryId);

                    //Category selectCategory = _applicationDbContext.Categories
                    //        .FirstOrDefault(c => c.CategoryId == user.SelectedCategoryId);



                    queueClient.SendMessage(JsonConvert.SerializeObject(user, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
                    //_logger.LogInformation($"Message to {user.Email} sent to queue with newsletter category: {queueUser.SelectedCategory?.Name}");

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