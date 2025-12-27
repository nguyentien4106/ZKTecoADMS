using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatemodel22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalancedPaidLeaveDays",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UsedPaidLeaveDays",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalancedPaidLeaveDays",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "UsedPaidLeaveDays",
                table: "EmployeeSalaryProfiles");
        }
    }
}
