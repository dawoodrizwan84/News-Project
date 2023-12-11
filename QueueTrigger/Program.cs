using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueueTrigger.Service;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    //.ConfigureServices(service =>
    //{

    //    service.AddScoped<IUserService, UserService>();
    //})
    .Build();

host.Run();
