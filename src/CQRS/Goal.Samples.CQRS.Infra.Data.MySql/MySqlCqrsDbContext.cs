using Microsoft.EntityFrameworkCore;

namespace Goal.Samples.CQRS.Infra.Data.MySql;

public class MySqlCqrsDbContext : CqrsDbContext
{
    public MySqlCqrsDbContext(DbContextOptions options)
        : base(options)
    { }
}
