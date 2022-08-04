using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Goal.Demo2.Infra.Crosscutting.Extensions;
using Goal.Demo2.Infra.Extensions;
using Goal.Demo2.Infra.Support;
using Goal.Demo2.Infra.Swagger;
using Goal.Seedwork.Infra.Crosscutting.Localization;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.ConfgureLogging(builder.Configuration, builder.Environment));

// Add services to the container.
builder.Services.AddServices(builder.Configuration, builder.Environment);
builder.Services.AddAutoMapperTypeAdapter();
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

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
        options.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo2 Api v1");
        c.DisplayRequestDuration();
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

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
app.Run();
