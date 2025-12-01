using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShiftCheckInCheckOutAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CheckInAttendanceId",
                table: "Shifts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CheckOutAttendanceId",
                table: "Shifts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_CheckInAttendanceId",
                table: "Shifts",
                column: "CheckInAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_CheckOutAttendanceId",
                table: "Shifts",
                column: "CheckOutAttendanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckInAttendanceId",
                table: "Shifts",
                column: "CheckInAttendanceId",
                principalTable: "AttendanceLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckOutAttendanceId",
                table: "Shifts",
                column: "CheckOutAttendanceId",
                principalTable: "AttendanceLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckInAttendanceId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckOutAttendanceId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_CheckInAttendanceId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_CheckOutAttendanceId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "CheckInAttendanceId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "CheckOutAttendanceId",
                table: "Shifts");
        }
    }
}
