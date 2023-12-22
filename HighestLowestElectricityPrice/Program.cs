using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker;
using HighestLowestElectricityPrice.Services;


var host = new HostBuilder()
     .ConfigureAppConfiguration(builder =>
            builder.AddJsonFile("local.settings.json",true,true))
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s =>
    {
        s.AddScoped<ISpotService, SpotService>();
        //s.AddHttpClient();
    })
    //.ConfigureServices(services =>
    //{
    //    // Add Application Insights telemetry for Azure Functions
    //    services.AddApplicationInsightsTelemetryWorkerService();

    //    // Additional configuration for Application Insights if needed
    //    services.ConfigureFunctionsApplicationInsights();
    //})

    .Build();

host.Run();