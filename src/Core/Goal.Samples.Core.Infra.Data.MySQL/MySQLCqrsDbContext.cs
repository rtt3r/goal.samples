using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.Core.Infra.Data.MySQL;

public class MySQLCqrsDbContext : CqrsDbContext
{
    public MySQLCqrsDbContext(DbContextOptions options)
        : base(options)
    { }
}
