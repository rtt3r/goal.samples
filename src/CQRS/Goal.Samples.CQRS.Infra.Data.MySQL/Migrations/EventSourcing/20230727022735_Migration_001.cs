using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Goal.Samples.CQRS.Infra.Data.MySQL.Migrations.EventSourcing
{
    /// <inheritdoc />
    public partial class Migration_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoredEvents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Data = table.Column<string>(type: "longtext", nullable: true),
                    User = table.Column<string>(type: "longtext", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    AggregateId = table.Column<string>(type: "longtext", nullable: true),
                    EventType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredEvents", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoredEvents");
        }
    }
}
