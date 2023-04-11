using System.Security.Cryptography.X509Certificates;
using Goal.Samples.Identity.Data;
using Goal.Samples.Identity.Infra;
using Goal.Samples.Identity.Models;
using Goal.Samples.Identity.Settings;
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
        var signinCredentials = new SigninCredentialsSettings();
        configuration.Bind("SigninCredentials", signinCredentials);

        X509Certificate2 certificate = signinCredentials.Thumbprint is not null
            ? LoadCertificateFromStore(StoreName.My, StoreLocation.CurrentUser, signinCredentials.Thumbprint)
            : LoadCertificateFromPath(signinCredentials.Path, signinCredentials.Password);

        builder.AddSigningCredential(certificate);
        return builder;
    }

    private static X509Certificate2 LoadCertificateFromStore(StoreName storeName, StoreLocation storeLocation, string thumbprint)
    {
        using var certStore = new X509Store(storeName, storeLocation);
        certStore.Open(OpenFlags.ReadOnly);

        X509Certificate2Collection certCollection = certStore.Certificates.Find(
            X509FindType.FindByThumbprint,
            thumbprint,
            false);

        var certificate = certCollection.FirstOrDefault();

        if (certificate is not null)
        {
            return certificate;
        }

        throw new InvalidOperationException($"Fail to load certificate from registry: {thumbprint}");
    }

    private static X509Certificate2 LoadCertificateFromPath(string path, string password)
    {
        if (!File.Exists(path))
        {
            throw new InvalidOperationException($"Certificate not found at '{path}'.");
        }

        return new X509Certificate2(path, password);
    }
}