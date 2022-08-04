using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Goal.Demo2.Infra.Data.Migrations.Demo2Context
{
    public partial class UpdateBirthDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Customers",
                newName: "Birthdate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthdate",
                table: "Customers",
                newName: "BirthDate");
        }
    }
}
