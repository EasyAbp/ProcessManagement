using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyAbp.ProcessManagement.Migrations
{
    /// <inheritdoc />
    public partial class Removed_StateDetailsText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateDetailsText",
                table: "EasyAbpProcessManagementProcessStateHistories");

            migrationBuilder.DropColumn(
                name: "StateDetailsText",
                table: "EasyAbpProcessManagementProcesses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateDetailsText",
                table: "EasyAbpProcessManagementProcessStateHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateDetailsText",
                table: "EasyAbpProcessManagementProcesses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
