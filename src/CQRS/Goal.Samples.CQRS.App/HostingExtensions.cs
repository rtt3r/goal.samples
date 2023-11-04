using System.Globalization;
using System.Text.Json.Serialization;
using Goal.Samples.CQRS.App.Swagger;
using Goal.Samples.CQRS.Infra.IoC.Extensions;
using Goal.Samples.Infra.Crosscutting.Extensions;
using Goal.Samples.Infra.Crosscutting.Settings;
using Goal.Samples.Infra.Http.Filters;
using Goal.Samples.Infra.Http.JsonNamePolicies;
using Goal.Samples.Infra.Http.ValueProviders;
using Goal.Seedwork.Infra.Crosscutting.Localization;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Goal.Samples.CQRS.App;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, lc) => lc.ConfigureLogging(builder.Configuration, builder.Environment));

        builder.Services.ConfigureApiServices(builder.Configuration, builder.Environment);

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        builder.Services.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));
                cfg.UseDelayedMessageScheduler();
                cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));
                cfg.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
            });
        });

        builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<HttpExceptionFilter>();
                options.ValueProviderFactories.Add(new SnakeCaseQueryValueProviderFactory());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureApiSwaggerOptions>();
        builder.Services.AddSwaggerGen();

        KeycloakSettings keycloakOptions = builder.Configuration
            .GetSection(KeycloakSettings.Section)
            .Get<KeycloakSettings>();

        builder.Services.AddKeycloakAuthentication(keycloakOptions.AuthenticationOptions);
        builder.Services.AddKeycloakAuthorization(keycloakOptions.ProtectionClientOptions);
        builder.Services.AddKeycloakAdminHttpClient(keycloakOptions.AdminClientOptions);

        builder.Services.AddCors();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
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

        app.UseSerilogRequestLogging();
        app.MigrateApiDbContext();

        if (app.Environment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
            app.UseDeveloperExceptionPage();

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer
                        {
                            Url = $"{httpReq.Scheme}://{httpReq.Host.Value}"
                        }
                    };
                });
            });
            app.UseSwaggerUI(c =>
            {
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var groupName in apiVersionDescriptionProvider.ApiVersionDescriptions.Select(p => p.GroupName))
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{groupName}/swagger.json",
                        groupName.ToUpperInvariant());
                }

                c.OAuthClientId(app.Configuration["Keycloak:Resource"]);
                c.OAuthClientSecret(app.Configuration["Keycloak:Credentials:Secret"]);
                c.OAuthAppName("Goal CQRS Api");

                c.DisplayRequestDuration();
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();

        app.UseCors(builder =>
        {
            string[] origins = app.Configuration["Cors:Origins"].Split(';', StringSplitOptions.RemoveEmptyEntries);

            builder
                .WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });

        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}