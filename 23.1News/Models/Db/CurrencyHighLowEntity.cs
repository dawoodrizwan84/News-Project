using Azure;
using Azure.Data.Tables;

namespace _23._1News.Models.Db
{
    public class CurrencyHighLowEntity : ITableEntity
    {
        public string RowKey { get; set; } = default!;

        public decimal HighestPrice { get; set; }
        public decimal LowestProice { get; set; }

        public int HourHighest { get; set; }
        public decimal HourLowest { get; set; }

        public string PartitionKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }
    }
}
