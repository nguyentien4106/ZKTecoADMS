using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cascademode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceLogs_Employees_EmployeeId",
                table: "AttendanceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckInAttendanceId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckOutAttendanceId",
                table: "Shifts");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ManagerId",
                table: "AspNetUsers",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceLogs_Employees_EmployeeId",
                table: "AttendanceLogs",
                column: "EmployeeId",
                principalTable: "Employees",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceLogs_Employees_EmployeeId",
                table: "AttendanceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckInAttendanceId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AttendanceLogs_CheckOutAttendanceId",
                table: "Shifts");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ManagerId",
                table: "AspNetUsers",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceLogs_Employees_EmployeeId",
                table: "AttendanceLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

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
    }
}
