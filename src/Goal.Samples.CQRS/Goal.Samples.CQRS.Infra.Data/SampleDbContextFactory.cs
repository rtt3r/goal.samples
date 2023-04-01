using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data
{
    public class SampleDbContextFactory : DesignTimeDbContextFactory<SampleDbContext>
    {
        protected override SampleDbContext CreateNewInstance(DbContextOptionsBuilder<SampleDbContext> optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
            return new SampleDbContext(optionsBuilder.Options);
        }
    }
}
