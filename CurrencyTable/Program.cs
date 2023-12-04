using CurrencyTable.Properties.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
      .ConfigureAppConfiguration(builder =>
      {
          builder.AddJsonFile("local.settings.json", true, true);
      })

    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices( s =>
        {
            s.AddScoped<ICurrencyServices, CurrencyServices>();
            s.AddHttpClient();

        })

    .Build();

host.Run();
