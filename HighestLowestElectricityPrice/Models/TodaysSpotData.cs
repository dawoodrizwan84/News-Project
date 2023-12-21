using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighestLowestElectricityPrice.Models
{
    public class TodaysSpotData
    {
        public List<SpotDataList> TodaysSpotPrices { get; set; }
        public TodaysSpotData() 
        {
            TodaysSpotPrices = new List<SpotDataList>();
        }
    }
}
