using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addconfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceLogs_Devices_DeviceId",
                table: "AttendanceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceLogs_UserDevices_UserId",
                table: "AttendanceLogs");

            migrationBuilder.DropIndex(
                name: "IX_UserDeviceMappings_UserId",
                table: "UserDeviceMappings");

            migrationBuilder.DropIndex(
                name: "IX_FingerprintTemplates_UserId",
                table: "FingerprintTemplates");

            migrationBuilder.DropIndex(
                name: "IX_FaceTemplates_UserId",
                table: "FaceTemplates");

            migrationBuilder.DropIndex(
                name: "IX_DeviceSettings_DeviceId",
                table: "DeviceSettings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserDevices",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserDevices",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Devices",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Devices",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "AttendanceLogs",
                type: "decimal(4,1)",
                precision: 4,
                scale: 1,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_PIN",
                table: "UserDevices",
                column: "PIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDeviceMappings_UserId_DeviceId",
                table: "UserDeviceMappings",
                columns: new[] { "UserId", "DeviceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigurations_ConfigKey",
                table: "SystemConfigurations",
                column: "ConfigKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SyncLogs_CreatedAt",
                table: "SyncLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintTemplates_UserId_FingerIndex",
                table: "FingerprintTemplates",
                columns: new[] { "UserId", "FingerIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FaceTemplates_UserId_FaceIndex",
                table: "FaceTemplates",
                columns: new[] { "UserId", "FaceIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceSettings_DeviceId_SettingKey",
                table: "DeviceSettings",
                columns: new[] { "DeviceId", "SettingKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_SerialNumber",
                table: "Devices",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCommands_Status",
                table: "DeviceCommands",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_AttendanceTime",
                table: "AttendanceLogs",
                column: "AttendanceTime");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_PIN",
                table: "AttendanceLogs",
                column: "PIN");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceLogs_Devices_DeviceId",
                table: "AttendanceLogs",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceLogs_UserDevices_UserId",
                table: "AttendanceLogs",
                column: "UserId",
                principalTable: "UserDevices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceLogs_Devices_DeviceId",
                table: "AttendanceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceLogs_UserDevices_UserId",
                table: "AttendanceLogs");

            migrationBuilder.DropIndex(
                name: "IX_UserDevices_PIN",
                table: "UserDevices");

            migrationBuilder.DropIndex(
                name: "IX_UserDeviceMappings_UserId_DeviceId",
                table: "UserDeviceMappings");

            migrationBuilder.DropIndex(
                name: "IX_SystemConfigurations_ConfigKey",
                table: "SystemConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_SyncLogs_CreatedAt",
                table: "SyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_FingerprintTemplates_UserId_FingerIndex",
                table: "FingerprintTemplates");

            migrationBuilder.DropIndex(
                name: "IX_FaceTemplates_UserId_FaceIndex",
                table: "FaceTemplates");

            migrationBuilder.DropIndex(
                name: "IX_DeviceSettings_DeviceId_SettingKey",
                table: "DeviceSettings");

            migrationBuilder.DropIndex(
                name: "IX_Devices_SerialNumber",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_DeviceCommands_Status",
                table: "DeviceCommands");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceLogs_AttendanceTime",
                table: "AttendanceLogs");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceLogs_PIN",
                table: "AttendanceLogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserDevices",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserDevices",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Devices",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Devices",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "AttendanceLogs",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,1)",
                oldPrecision: 4,
                oldScale: 1,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDeviceMappings_UserId",
                table: "UserDeviceMappings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintTemplates_UserId",
                table: "FingerprintTemplates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceTemplates_UserId",
                table: "FaceTemplates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceSettings_DeviceId",
                table: "DeviceSettings",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceLogs_Devices_DeviceId",
                table: "AttendanceLogs",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceLogs_UserDevices_UserId",
                table: "AttendanceLogs",
                column: "UserId",
                principalTable: "UserDevices",
                principalColumn: "Id");
        }
    }
}
