using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Goal.Demo2.Infra.Data.Migrations.EventSourcingContext
{
    public partial class UpdateTimestampColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Timestamp",
                table: "StoredEvents",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "StoredEvents");
        }
    }
}
