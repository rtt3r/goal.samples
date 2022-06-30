using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace Goal.Demo2.Infra.Crosscutting.Extensions
{
    public static class LoggingExtensions
    {
        public static void ConfgureLogging(
            this LoggerConfiguration logger,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            logger
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithCorrelationId()
                .Enrich.WithEnvironmentName()
                .WriteTo.Debug(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .WriteTo.Seq(configuration.GetConnectionString("Seq"))
                .ReadFrom.Configuration(configuration);
        }
    }
}
