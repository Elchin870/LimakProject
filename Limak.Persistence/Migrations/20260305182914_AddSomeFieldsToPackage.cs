using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Limak.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeFieldsToPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminNote",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "Packages",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Length",
                table: "Packages",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "Packages",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminNote",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Packages");
        }
    }
}
