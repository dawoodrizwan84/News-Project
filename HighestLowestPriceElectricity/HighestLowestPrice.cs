using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace HighestLowestPriceElectricity
{
    public class HighestLowestPrice
    {
        [FunctionName("HighestLowestPrice")]
        public void Run([TimerTrigger("0 13 * * * *",RunOnStartup =true)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
