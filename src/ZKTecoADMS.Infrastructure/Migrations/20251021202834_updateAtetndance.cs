using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAtetndance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "AttendanceLogs");

            migrationBuilder.DropColumn(
                name: "MaskStatus",
                table: "AttendanceLogs");

            migrationBuilder.DropColumn(
                name: "ProcessedAt",
                table: "AttendanceLogs");

            migrationBuilder.DropColumn(
                name: "RawData",
                table: "AttendanceLogs");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "AttendanceLogs");

            migrationBuilder.DropColumn(
                name: "VerifyType",
                table: "AttendanceLogs");

            migrationBuilder.RenameColumn(
                name: "VerifyState",
                table: "AttendanceLogs",
                newName: "VerifyMode");

            migrationBuilder.AddColumn<int>(
                name: "AttendanceState",
                table: "AttendanceLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceState",
                table: "AttendanceLogs");

            migrationBuilder.RenameColumn(
                name: "VerifyMode",
                table: "AttendanceLogs",
                newName: "VerifyState");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "AttendanceLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MaskStatus",
                table: "AttendanceLogs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedAt",
                table: "AttendanceLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RawData",
                table: "AttendanceLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Temperature",
                table: "AttendanceLogs",
                type: "decimal(4,1)",
                precision: 4,
                scale: 1,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VerifyType",
                table: "AttendanceLogs",
                type: "int",
                nullable: true);
        }
    }
}
