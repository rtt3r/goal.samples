using Microsoft.EntityFrameworkCore.Migrations;


namespace Goal.Samples.Core.Infra.Data.MySql.Migrations.EventSourcing;

/// <inheritdoc />
public partial class Migration_000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:Charset", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
