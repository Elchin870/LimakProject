using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Limak.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "Packages",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "Packages");
        }
    }
}
