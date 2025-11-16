using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDevices_PIN",
                table: "UserDevices");

            migrationBuilder.DropIndex(
                name: "IX_DeviceCommands_Status",
                table: "DeviceCommands");

            migrationBuilder.RenameColumn(
                name: "PIN",
                table: "UserDevices",
                newName: "Pin");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "UserDevices",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "UserDevices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "UserDevices",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "UserDevices",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_Pin",
                table: "UserDevices",
                column: "Pin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDevices_Pin",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "UserDevices");

            migrationBuilder.RenameColumn(
                name: "Pin",
                table: "UserDevices",
                newName: "PIN");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_PIN",
                table: "UserDevices",
                column: "PIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCommands_Status",
                table: "DeviceCommands",
                column: "Status");
        }
    }
}
