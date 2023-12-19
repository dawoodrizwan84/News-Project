using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsLetterToQueue.Services;
using Queue_NoReference.Data;
using Queue_NoReference.Services;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile("local.settings.json", true, true);
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((ctx, s) =>
    {
        var connectionString = ctx.Configuration["DefaultConnection"]
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        s.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        s.AddScoped<IUserQServices, UserQServicescs>();

    })
    .Build();
host.Run();


