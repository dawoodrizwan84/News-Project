using CurrencyTable.Properties.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTable.Properties.Services
{
    public interface ICurrencyServices
    {
        Task<string> GetRateAsync();

        Task SaveDataAsync(CurrencyRates newtodaysRates);

     
        //Task<CurrencyExchangeRates> GetRate();
    }
}
