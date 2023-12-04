using Azure;
using Azure.Data.Tables;

namespace _23._1News.Models.Db
{
    public class TodaysRateEntity : ITableEntity
    {
        public string RowKey { get; set; } = string.Empty;
        public string PartitionKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.Now;
        public ETag ETag { get; set; } = ETag.All;
        public string Currency { get; set; }
        public decimal Rate { get; set; }


       

    }
}
