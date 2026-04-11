using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Limak.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OfficeId",
                table: "Packages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_OfficeId",
                table: "Packages",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Offices_OfficeId",
                table: "Packages",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Offices_OfficeId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_OfficeId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "Packages");
        }
    }
}
