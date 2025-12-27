using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatemodel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaryProfiles_SalaryProfiles_SalaryProfileId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryProfiles_EffectiveDate",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryProfiles_EmployeeId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryProfiles_EmployeeId_IsActive",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryProfiles_SalaryProfileId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "SalaryProfileId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryProfiles_EmployeeId",
                table: "EmployeeSalaryProfiles",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryProfiles_EmployeeId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "SalaryProfileId",
                table: "EmployeeSalaryProfiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryProfiles_EffectiveDate",
                table: "EmployeeSalaryProfiles",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryProfiles_EmployeeId",
                table: "EmployeeSalaryProfiles",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryProfiles_EmployeeId_IsActive",
                table: "EmployeeSalaryProfiles",
                columns: new[] { "EmployeeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryProfiles_SalaryProfileId",
                table: "EmployeeSalaryProfiles",
                column: "SalaryProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaryProfiles_SalaryProfiles_SalaryProfileId",
                table: "EmployeeSalaryProfiles",
                column: "SalaryProfileId",
                principalTable: "SalaryProfiles",
                principalColumn: "Id");
        }
    }
}
