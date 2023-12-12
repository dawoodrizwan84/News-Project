using _23._1News.Data;
using _23._1News.Models.Db;
using _23._1News.Services.Abstract;
using _23._1News.Services.Implement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _23._1News.Controllers
{
   
    public class ElectricityPriceController:Controller
        
    {
        private readonly IElectricityService _electricityService;
        public ElectricityPriceController(IElectricityService electricityService)
        {

            _electricityService = electricityService;

        }
        public async Task<IActionResult> GetElectricityPrice()
        {
            var electricityprice = await _electricityService.GetElectricityPrice();
            return View(electricityprice);
        }


    }
}
