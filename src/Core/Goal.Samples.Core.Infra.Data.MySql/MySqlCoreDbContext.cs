using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.MySql;

public class MySqlCoreDbContext : CoreDbContext
{
    public MySqlCoreDbContext(DbContextOptions options)
        : base(options)
    { }
}
