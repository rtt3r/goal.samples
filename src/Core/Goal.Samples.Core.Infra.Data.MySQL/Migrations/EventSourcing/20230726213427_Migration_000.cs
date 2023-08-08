using Microsoft.EntityFrameworkCore.Migrations;


namespace Goal.Samples.Core.Infra.Data.MySQL.Migrations.EventSourcing;

/// <inheritdoc />
public partial class Migration_000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySQL:Charset", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
