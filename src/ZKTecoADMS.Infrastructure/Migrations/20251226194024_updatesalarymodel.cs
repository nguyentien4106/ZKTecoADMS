using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatesalarymodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaryProfiles_SalaryProfiles_SalaryProfileId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "SalaryProfileId",
                table: "EmployeeSalaryProfiles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaryProfiles_SalaryProfiles_SalaryProfileId",
                table: "EmployeeSalaryProfiles",
                column: "SalaryProfileId",
                principalTable: "SalaryProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaryProfiles_SalaryProfiles_SalaryProfileId",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "SalaryProfileId",
                table: "EmployeeSalaryProfiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaryProfiles_SalaryProfiles_SalaryProfileId",
                table: "EmployeeSalaryProfiles",
                column: "SalaryProfileId",
                principalTable: "SalaryProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
