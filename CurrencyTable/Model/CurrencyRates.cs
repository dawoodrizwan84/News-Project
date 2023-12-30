using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTable.Model
{

    public class CurrencyRates
    {
        public string provider { get; set; }
        public string WARNING_UPGRADE_TO_V6 { get; set; }
        public string terms { get; set; }
        public string _base { get; set; }
        public string Date { get; set; }
        public int time_last_updated { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }

    }



}
