using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Ritter.Starter.Identity.Data
{
    public class ApplicationDbContextDesignTimeFactory : DesignTimeDbContextFactory<ApplicationDbContext>
    {
        protected override ApplicationDbContext CreateNewInstance(DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
