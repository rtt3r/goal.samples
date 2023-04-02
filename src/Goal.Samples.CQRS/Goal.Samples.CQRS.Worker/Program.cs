using Goal.Samples.CQRS.Infra.IoC.Extensions;
using Goal.Samples.CQRS.Worker.Services;
using Goal.Samples.Infra.Crosscutting.Extensions;
using Microsoft.AspNetCore.Hosting;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        services.AddLogging(configure => configure.AddSerilog());
        services.ConfigureWorkerServices(configuration);
        services.AddHostedService<ScopedBackgroundService>();
    })
    .UseSerilog((ctx, lc) => lc.ConfigureLogging(ctx.Configuration, null))
    .Build();

host.Run();