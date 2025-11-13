using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceLogs_Devices_DeviceId",
                table: "AttendanceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDevices_Devices_DeviceId",
                table: "UserDevices");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceLogs_AttendanceTime",
                table: "AttendanceLogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AttendanceTime",
                table: "AttendanceLogs",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceLogs_Devices_DeviceId",
                table: "AttendanceLogs",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDevices_Devices_DeviceId",
                table: "UserDevices",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceLogs_Devices_DeviceId",
                table: "AttendanceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDevices_Devices_DeviceId",
                table: "UserDevices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AttendanceTime",
                table: "AttendanceLogs",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_AttendanceTime",
                table: "AttendanceLogs",
                column: "AttendanceTime");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceLogs_Devices_DeviceId",
                table: "AttendanceLogs",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDevices_Devices_DeviceId",
                table: "UserDevices",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
