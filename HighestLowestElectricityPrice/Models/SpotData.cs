using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighestLowestElectricityPrice.Models
{
    public class SpotData
    {
        public DateTime DateTime { get; set; }=DateTime.Now;
        public string AreaName { get; set; } = "";
        public string Price { get; set; } = "";
        public decimal PriceDecimal { get; set; }
    }
}
