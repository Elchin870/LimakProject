using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Limak.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerCode",
                table: "Users",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CustomerCode",
                table: "Users",
                column: "CustomerCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_CustomerCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CustomerCode",
                table: "Users");
        }
    }
}
