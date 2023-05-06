using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data
{
    public class CqrsDbContextFactory : DesignTimeDbContextFactory<CqrsDbContext>
    {
        protected override CqrsDbContext CreateNewInstance(DbContextOptionsBuilder<CqrsDbContext> optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
            return new CqrsDbContext(optionsBuilder.Options);
        }
    }
}
