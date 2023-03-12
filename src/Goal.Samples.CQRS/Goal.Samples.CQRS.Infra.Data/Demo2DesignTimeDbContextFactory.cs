using Goal.Seedwork.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data
{
    public class Demo2DesignTimeDbContextFactory : DesignTimeDbContextFactory<Demo2Context>
    {
        protected override Demo2Context CreateNewInstance(DbContextOptionsBuilder<Demo2Context> optionsBuilder, string connectionString)
        {
            optionsBuilder.UseSqlServer(connectionString);
            return new Demo2Context(optionsBuilder.Options);
        }
    }
}
