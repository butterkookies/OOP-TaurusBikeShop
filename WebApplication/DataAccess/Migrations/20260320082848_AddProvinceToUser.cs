using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProvinceToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Province",
                table: "User");
        }
    }
}
