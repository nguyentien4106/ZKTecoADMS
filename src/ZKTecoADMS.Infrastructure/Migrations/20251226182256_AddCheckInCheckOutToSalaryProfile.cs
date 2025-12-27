using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckInCheckOutToSalaryProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceUsers_Employees_EmployeeId",
                table: "DeviceUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SalaryPerHour",
                table: "SalaryProfiles");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckIn",
                table: "SalaryProfiles",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckOut",
                table: "SalaryProfiles",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                table: "DeviceUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceUsers_EmployeeId1",
                table: "DeviceUsers",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceUsers_Employees_EmployeeId",
                table: "DeviceUsers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceUsers_Employees_EmployeeId1",
                table: "DeviceUsers",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceUsers_Employees_EmployeeId",
                table: "DeviceUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceUsers_Employees_EmployeeId1",
                table: "DeviceUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_DeviceUsers_EmployeeId1",
                table: "DeviceUsers");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "DeviceUsers");

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryPerHour",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceUsers_Employees_EmployeeId",
                table: "DeviceUsers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
