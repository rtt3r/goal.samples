using System.Globalization;
using System.Text.Json.Serialization;
using Goal.Samples.CQRS.Infra.IoC.Extensions;
using Goal.Samples.CQRS.Worker.Consumers.Customers;
using Goal.Samples.CQRS.Worker.Infra.Swagger;
using Goal.Samples.Infra.Crosscutting.Extensions;
using Goal.Seedwork.Infra.Crosscutting.Localization;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Goal.Samples.CQRS.Worker;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc.ConfigureLogging(builder.Configuration, builder.Environment));

        builder.Services.ConfigureWorkerServices(builder.Configuration, builder.Environment);

        builder.Services.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.AddConsumer<CustomerRegisteredEventConsumer>(typeof(CustomerRegisteredEventConsumer.ConsumerDefinition));
            x.AddConsumer<CustomerRemovedEventConsumer>(typeof(CustomerRemovedEventConsumer.ConsumerDefinition));
            x.AddConsumer<CustomerUpdatedEventConsumer>(typeof(CustomerUpdatedEventConsumer.ConsumerDefinition));

            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
                cfg.UseDelayedMessageScheduler();
                cfg.ServiceInstance(instance =>
                {
                    instance.ConfigureJobServiceEndpoints();
                    instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                });
            });
        });

        builder.Services
            .AddRouting(options =>
            {
                options.LowercaseUrls = true;
            })
            .AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.MigrateWorkerDbContext();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Goal Samples CQRS Worker v1");
                c.DisplayRequestDuration();
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(ApplicationCultures.Portugues, ApplicationCultures.Portugues),
            SupportedCultures = new List<CultureInfo>
            {
                ApplicationCultures.Portugues,
            },
            SupportedUICultures = new List<CultureInfo>
            {
                ApplicationCultures.Portugues,
            }
        });

        app.MapControllers();

        return app;
    }
}