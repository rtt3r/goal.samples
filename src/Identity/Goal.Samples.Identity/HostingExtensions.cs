using System.Security.Cryptography.X509Certificates;
using Goal.Samples.Identity.Data;
using Goal.Samples.Identity.Infra;
using Goal.Samples.Identity.Models;
using Goal.Samples.Infra.Crosscutting.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Goal.Samples.Identity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc.ConfigureLogging(builder.Configuration, builder.Environment));

        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services
            .AddScoped<CustomUserManager>()
            .AddScoped<CustomUserStore>()
            .AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsPrincipalFactory>();

        builder.Services
            .AddIdentity<User, Role>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequiredLength = 8;
                o.Password.RequiredUniqueChars = 1;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = true;
                o.User.RequireUniqueEmail = false;
                o.SignIn.RequireConfirmedEmail = false;
            })
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<CustomUserManager>()
            .AddUserStore<CustomUserStore>()
            .AddRoleManager<CustomRoleManager>()
            .AddRoleStore<CustomRoleStore>()
            .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddSigningCertificate(builder.Configuration)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = opts =>
                    opts.UseSqlServer(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

                options.DefaultSchema = "Identity";
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = opts =>
                    opts.UseSqlServer(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

                options.DefaultSchema = "Identity";
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<User>()
            .AddProfileService<CustomProfileService>()
            .AddCustomTokenRequestValidator<CustomTokenRequestValidator>();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }

    private static IIdentityServerBuilder AddSigningCertificate(this IIdentityServerBuilder builder, IConfiguration configuration)
    {
        X509Certificate2 certificate;

        string certThumbprint = configuration["Certificates:SigninCredentials:Thumbprint"];

        if (!string.IsNullOrWhiteSpace(certThumbprint))
        {
            using var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = certStore.Certificates.Find(
                X509FindType.FindByThumbprint,
                certThumbprint,
                false);

            if (certCollection.Count > 0)
            {
                certificate = certCollection[0];
                Log.Logger.Information($"Successfully loaded cert from registry: {certificate.Thumbprint}");

                builder.AddSigningCredential(certificate);
                return builder;
            }
        }

        string certPath = configuration["Certificates:SigninCredentials:Path"];
        string certPass = configuration["Certificates:SigninCredentials:Password"];

        Log.Logger.Information($"Loadind certificate from path: '{certPath}'");

        if (!File.Exists(certPath))
        {
            throw new Exception($"Certificate not found at '{certPath}'.");
        }

        certificate = new X509Certificate2(certPath, certPass);

        if (certificate is null)
        {
            throw new Exception($"Certificate '{certPath}' was not found.");
        }

        builder.AddSigningCredential(certificate);
        return builder;
    }
}