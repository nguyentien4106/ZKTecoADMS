using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_SalaryProfiles_BenefitId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AspNetUsers_EmployeeUserId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckInAttendanceId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckOutAttendanceId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Employees_BenefitId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MaximumAllowedEarlyLeaveMinutes",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "MaximumAllowedLateMinutes",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "BenefitId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "EmployeeUserId",
                table: "Shifts",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_EmployeeUserId",
                table: "Shifts",
                newName: "IX_Shifts_EmployeeId");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId1",
                table: "Shifts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ApplicationUserId1",
                table: "Shifts",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryProfiles_Name",
                table: "SalaryProfiles",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AspNetUsers_ApplicationUserId1",
                table: "Shifts",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckInAttendanceId",
                table: "Shifts",
                column: "CheckInAttendanceId",
                principalTable: "AttendanceLogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckOutAttendanceId",
                table: "Shifts",
                column: "CheckOutAttendanceId",
                principalTable: "AttendanceLogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Employees_EmployeeId",
                table: "Shifts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AspNetUsers_ApplicationUserId1",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckInAttendanceId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckOutAttendanceId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Employees_EmployeeId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_ApplicationUserId1",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_SalaryProfiles_Name",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Shifts",
                newName: "EmployeeUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_EmployeeId",
                table: "Shifts",
                newName: "IX_Shifts_EmployeeUserId");

            migrationBuilder.AddColumn<int>(
                name: "MaximumAllowedEarlyLeaveMinutes",
                table: "Shifts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaximumAllowedLateMinutes",
                table: "Shifts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BenefitId",
                table: "Employees",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BenefitId",
                table: "Employees",
                column: "BenefitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_SalaryProfiles_BenefitId",
                table: "Employees",
                column: "BenefitId",
                principalTable: "SalaryProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AspNetUsers_EmployeeUserId",
                table: "Shifts",
                column: "EmployeeUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckInAttendanceId",
                table: "Shifts",
                column: "CheckInAttendanceId",
                principalTable: "AttendanceLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckOutAttendanceId",
                table: "Shifts",
                column: "CheckOutAttendanceId",
                principalTable: "AttendanceLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
