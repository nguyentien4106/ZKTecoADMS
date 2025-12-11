using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorPayslipToUseEmployeeUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_Employees_EmployeeId",
                table: "Payslips");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Payslips",
                newName: "EmployeeUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payslips_EmployeeId_Year_Month",
                table: "Payslips",
                newName: "IX_Payslips_EmployeeUserId_Year_Month");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_AspNetUsers_EmployeeUserId",
                table: "Payslips",
                column: "EmployeeUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_AspNetUsers_EmployeeUserId",
                table: "Payslips");

            migrationBuilder.RenameColumn(
                name: "EmployeeUserId",
                table: "Payslips",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Payslips_EmployeeUserId_Year_Month",
                table: "Payslips",
                newName: "IX_Payslips_EmployeeId_Year_Month");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_Employees_EmployeeId",
                table: "Payslips",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
