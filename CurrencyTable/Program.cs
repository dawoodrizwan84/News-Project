using _23._1News.Services.Abstract;
using _23._1News.Services.Implement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
      .ConfigureAppConfiguration(builder =>
      {
          builder.AddJsonFile("local.settings.json", true, true);
      })

    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
        {
            s.AddScoped<ICurrencyExchange, CurrencyExchange>();
        })

    .Build();

host.Run();
