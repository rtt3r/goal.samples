using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.MySQL;

public class MySQLCqrsDbContext : CqrsDbContext
{
    public MySQLCqrsDbContext(DbContextOptions options)
        : base(options)
    { }
}
