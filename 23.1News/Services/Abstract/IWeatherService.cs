
﻿using _23._1News.Controllers;
using _23._1News.Models.Db;
>

namespace _23._1News.Services.Abstract
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetWeatherForecast(string city);
        //List<string> GetCities();

    }
}
