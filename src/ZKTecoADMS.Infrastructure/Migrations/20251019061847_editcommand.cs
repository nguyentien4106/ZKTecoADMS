using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editcommand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommandData",
                table: "DeviceCommands");

            migrationBuilder.DropColumn(
                name: "CommandType",
                table: "DeviceCommands");

            migrationBuilder.AddColumn<string>(
                name: "Command",
                table: "DeviceCommands",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Command",
                table: "DeviceCommands");

            migrationBuilder.AddColumn<string>(
                name: "CommandData",
                table: "DeviceCommands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommandType",
                table: "DeviceCommands",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
