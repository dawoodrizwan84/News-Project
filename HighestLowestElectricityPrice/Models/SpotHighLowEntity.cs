using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighestLowestElectricityPrice.Models
{
    public class SpotHighLowEntity:ITableEntity
    {
        public string RowKey { get; set; } = default!;
        public string PartitionKey { get; set; }= default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
        public ETag ETag { get; set; } = default!;
        public decimal HighestPrice { get; set; }

        public int HourHighest { get; set; }
        public decimal LowestPrice { get; set; }
        public int HourLowest { get; set; }
    }
}
