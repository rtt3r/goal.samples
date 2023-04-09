using System.Globalization;
using System.Text.Json.Serialization;
using Goal.Samples.CQRS.Api.Swagger;
using Goal.Samples.CQRS.Infra.IoC.Extensions;
using Goal.Samples.Infra.Crosscutting.Extensions;
using Goal.Samples.Infra.Http.Filters;
using Goal.Samples.Infra.Http.JsonNamePolicies;
using Goal.Samples.Infra.Http.ValueProviders;
using Goal.Seedwork.Infra.Crosscutting.Localization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Goal.Samples.CQRS.Api;

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
                options.ValueProviderFactories.Clear();
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

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                string authority = builder.Configuration["Auth:Authority"] ?? string.Empty;

                options.Authority = authority;
                options.RequireHttpsMetadata = authority.StartsWith("https://");
                options.TokenValidationParameters.ValidateAudience = false;
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("goal.read", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "goal.read", "manage");
            });

            options.AddPolicy("goal.write", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "goal.write", "manage");
            });
        });

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
                foreach (ApiVersionDescription description in app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                c.OAuthClientId(app.Configuration["Auth:ClientId"]);
                c.OAuthClientSecret(app.Configuration["Auth:ClientSecret"]);
                c.OAuthAppName("Goal Samples Api");

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
        app.MapControllers().RequireAuthorization();

        return app;
    }
}