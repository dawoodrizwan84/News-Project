using HighestLowestElectricityPrice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighestLowestElectricityPrice.Services
{
    public interface ISpotService
    {
      Task<TodaysSpotData> GetSpotMetrics();
        void SaveSportData(TodaysSpotData todayData);
    }
}
