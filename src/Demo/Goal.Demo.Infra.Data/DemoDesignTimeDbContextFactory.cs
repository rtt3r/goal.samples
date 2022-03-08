using Goal.Infra.Data.Seedwork;
using Microsoft.EntityFrameworkCore;

namespace Goal.Demo.Infra.Data
{
    public class DemoDesignTimeDbContextFactory : DesignTimeDbContextFactory<DemoContext>
    {
        protected override DemoContext CreateNewInstance(DbContextOptionsBuilder<DemoContext> optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlite(connectionString);
            return new DemoContext(optionsBuilder.Options);
        }
    }
}
