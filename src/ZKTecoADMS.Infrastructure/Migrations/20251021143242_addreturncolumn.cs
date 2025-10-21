using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addreturncolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDatetime",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "StartDatetime",
                table: "UserDevices");

            migrationBuilder.AddColumn<int>(
                name: "Return",
                table: "DeviceCommands",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Return",
                table: "DeviceCommands");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDatetime",
                table: "UserDevices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "UserDevices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDatetime",
                table: "UserDevices",
                type: "datetime2",
                nullable: true);
        }
    }
}
