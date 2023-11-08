using Microsoft.EntityFrameworkCore.Migrations;


namespace Goal.Samples.Core.Infra.Data.MySql.Migrations.Core;

/// <inheritdoc />
public partial class Migration_001 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Customers",
            columns: table => new
            {
                Id = table.Column<string>(type: "varchar(255)", nullable: false),
                Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                Email = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                BirthDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customers", x => x.Id);
            })
            .Annotation("MySql:Charset", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Customers");
    }
}
