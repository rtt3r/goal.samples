using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Goal.Samples.Core.Infra.Data.Npgsql.Migrations.Core
{
    /// <inheritdoc />
    public partial class Migration_002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Customers",
                newName: "Birthdate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthdate",
                table: "Customers",
                newName: "BirthDate");
        }
    }
}
