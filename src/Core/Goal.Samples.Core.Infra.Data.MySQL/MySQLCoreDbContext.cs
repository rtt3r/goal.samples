using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.MySQL;

public class MySQLCoreDbContext : CoreDbContext
{
    public MySQLCoreDbContext(DbContextOptions options)
        : base(options)
    { }
}
