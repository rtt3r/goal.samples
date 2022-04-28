using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace Goal.Demo2.Api.Infra.Extensions
{
    public static class LoggingExtensions
    {
        public static void ConfgureLogging(
            this LoggerConfiguration logger,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name.Replace(".", "-");
            string envName = environment.EnvironmentName.Replace(".", "-");

            logger
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithCorrelationId()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", $"{appName} - {envName}")
                .WriteTo.Debug()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .WriteTo.Seq(configuration.GetConnectionString("Seq"))
                .ReadFrom.Configuration(configuration);
        }
    }
}
