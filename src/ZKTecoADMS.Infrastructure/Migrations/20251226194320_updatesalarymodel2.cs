using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatesalarymodel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaryProfiles_DeviceUsers_EmployeeId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceUserId",
                table: "EmployeeSalaryProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryProfiles_DeviceUserId",
                table: "EmployeeSalaryProfiles",
                column: "DeviceUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaryProfiles_DeviceUsers_DeviceUserId",
                table: "EmployeeSalaryProfiles",
                column: "DeviceUserId",
                principalTable: "DeviceUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaryProfiles_Employees_EmployeeId",
                table: "EmployeeSalaryProfiles",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaryProfiles_DeviceUsers_DeviceUserId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaryProfiles_Employees_EmployeeId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryProfiles_DeviceUserId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "DeviceUserId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaryProfiles_DeviceUsers_EmployeeId",
                table: "EmployeeSalaryProfiles",
                column: "EmployeeId",
                principalTable: "DeviceUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
