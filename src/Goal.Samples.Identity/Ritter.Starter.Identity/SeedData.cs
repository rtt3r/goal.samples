using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Ritter.Starter.Identity.Data;
using Ritter.Starter.Identity.Models;
using Serilog;

namespace Ritter.Starter.Identity
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            MigrateDbContext<ApplicationDbContext>(scope.ServiceProvider);
            MigrateDbContext<ConfigurationDbContext>(scope.ServiceProvider);
            MigrateDbContext<PersistedGrantDbContext>(scope.ServiceProvider);

            ApplicationDbContext applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            ConfigurationDbContext configurationContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            PersistedGrantDbContext persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

            UserManager<User> userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            User admin = userMgr.FindByNameAsync("admin").Result;

            if (admin == null)
            {
                admin = new User
                {
                    UserName = "admin",
                    Name = "Administrator",
                    Email = "admin@ritter.com",
                    EmailConfirmed = true
                };

                IdentityResult result = userMgr.CreateAsync(admin, "Pass123$").Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("admin created");
            }
            else
            {
                Log.Debug("admin already exists");
            }

            RoleManager<Role> roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            Role administrator = roleMgr.FindByNameAsync("Administrator").Result;

            if (administrator == null)
            {
                administrator = new Role
                {
                    Name = "Administrator"
                };

                IdentityResult result = roleMgr.CreateAsync(administrator).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("Administrator created");
            }
            else
            {
                Log.Debug("Administrator already exists");
            }

            SeedIdentityResources(configurationContext);
            SeedApiScopes(configurationContext);
            SeedApiResources(configurationContext);
            SeedClients(configurationContext);
        }

        private static void SeedApiScopes(ConfigurationDbContext configurationContext)
        {
            foreach (Duende.IdentityServer.Models.ApiScope scope in Config.ApiScopes)
            {
                if (!configurationContext.ApiScopes.Any(c => c.Name == scope.Name))
                {
                    configurationContext.ApiScopes.Add(scope.ToEntity());
                }
            }

            configurationContext.SaveChanges();
        }

        private static void SeedApiResources(ConfigurationDbContext configurationContext)
        {
            foreach (Duende.IdentityServer.Models.ApiResource resource in Config.ApiResources)
            {
                if (!configurationContext.ApiResources.Any(c => c.Name == resource.Name))
                {
                    configurationContext.ApiResources.Add(resource.ToEntity());
                }
            }

            configurationContext.SaveChanges();
        }

        private static void SeedIdentityResources(ConfigurationDbContext configurationContext)
        {
            foreach (Duende.IdentityServer.Models.IdentityResource resource in Config.IdentityResources)
            {
                if (!configurationContext.IdentityResources.Any(c => c.Name == resource.Name))
                {
                    configurationContext.IdentityResources.Add(resource.ToEntity());
                }
            }

            configurationContext.SaveChanges();
        }

        private static void SeedClients(ConfigurationDbContext configurationContext)
        {
            Duende.IdentityServer.EntityFramework.Entities.Client dbClient;
            Duende.IdentityServer.EntityFramework.Entities.Client entity;

            foreach (Duende.IdentityServer.Models.Client client in Config.Clients)
            {
                dbClient = configurationContext.Clients
                    .Include(p => p.AllowedCorsOrigins)
                    .Include(p => p.Claims)
                    .Include(p => p.AllowedScopes)
                    .Include(p => p.PostLogoutRedirectUris)
                    .Include(p => p.RedirectUris)
                    .Include(p => p.Properties)
                    .FirstOrDefault(c => c.ClientId == client.ClientId);

                if (dbClient is null)
                {
                    configurationContext.Clients.Add(client.ToEntity());
                }
                else
                {
                    entity = client.ToEntity();

                    entity.AllowedCorsOrigins.ForEach(x =>
                    {
                        if (!dbClient.AllowedCorsOrigins.Any(c => c.Origin == x.Origin))
                        {
                            dbClient.AllowedCorsOrigins.Add(x);
                        }
                    });

                    entity.Claims.ForEach(x =>
                    {
                        if (!dbClient.Claims.Any(c => c.Type == x.Type))
                        {
                            dbClient.Claims.Add(x);
                        }
                    });

                    entity.AllowedScopes.ForEach(x =>
                    {
                        if (!dbClient.AllowedScopes.Any(c => c.Scope == x.Scope))
                        {
                            dbClient.AllowedScopes.Add(x);
                        }
                    });

                    entity.PostLogoutRedirectUris.ForEach(x =>
                    {
                        if (!dbClient.PostLogoutRedirectUris.Any(c => c.PostLogoutRedirectUri == x.PostLogoutRedirectUri))
                        {
                            dbClient.PostLogoutRedirectUris.Add(x);
                        }
                    });

                    entity.RedirectUris.ForEach(x =>
                    {
                        if (!dbClient.RedirectUris.Any(c => c.RedirectUri == x.RedirectUri))
                        {
                            dbClient.RedirectUris.Add(x);
                        }
                    });

                    entity.Properties.ForEach(x =>
                    {
                        if (!dbClient.Properties.Any(c => c.Key == x.Key))
                        {
                            dbClient.Properties.Add(x);
                        }
                    });

                    configurationContext.Clients.Update(dbClient);
                }
            }

            configurationContext.SaveChanges();
        }

        private static void MigrateDbContext<TContext>(IServiceProvider serviceProvider)
            where TContext : DbContext
        {
            Log.Information($"Applying migrations for '{typeof(TContext).Name}'.");

            DbContext context = serviceProvider.GetRequiredService<TContext>();

            context.Database.EnsureCreated();

            if (context.Database.GetPendingMigrations().Any())
            {
                Log.Information("Pending migrations found.");

                IMigrator migrator = context.Database.GetInfrastructure().GetService<IMigrator>();

                foreach (string migration in context.Database.GetPendingMigrations())
                {
                    Log.Information($"Applying migration '{migration}'.");

                    try
                    {
                        migrator.Migrate(migration);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Error when applying migration '{migration}'.");
                    }
                }
            }
            else
            {
                Log.Information($"No pending migrations found.");
            }
        }
    }
}