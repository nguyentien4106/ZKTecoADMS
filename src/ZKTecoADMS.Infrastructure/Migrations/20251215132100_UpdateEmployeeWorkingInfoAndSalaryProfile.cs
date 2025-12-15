using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeWorkingInfoAndSalaryProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficialHolidaysPerYear",
                table: "EmployeeWorkingInfos");

            migrationBuilder.RenameColumn(
                name: "TotalLeaveDays",
                table: "EmployeeWorkingInfos",
                newName: "BalancedUnpaidLeaveDays");

            migrationBuilder.RenameColumn(
                name: "BalancedLeaveDays",
                table: "EmployeeWorkingInfos",
                newName: "BalancedPaidLeaveDays");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnpaidLeaveDays",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidLeaveDays",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnpaidLeaveDaysPerYear",
                table: "EmployeeWorkingInfos",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidLeaveDaysPerYear",
                table: "EmployeeWorkingInfos",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BalancedUnpaidLeaveDays",
                table: "EmployeeWorkingInfos",
                newName: "TotalLeaveDays");

            migrationBuilder.RenameColumn(
                name: "BalancedPaidLeaveDays",
                table: "EmployeeWorkingInfos",
                newName: "BalancedLeaveDays");

            migrationBuilder.AlterColumn<int>(
                name: "UnpaidLeaveDays",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaidLeaveDays",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnpaidLeaveDaysPerYear",
                table: "EmployeeWorkingInfos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaidLeaveDaysPerYear",
                table: "EmployeeWorkingInfos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OfficialHolidaysPerYear",
                table: "EmployeeWorkingInfos",
                type: "integer",
                nullable: true);
        }
    }
}
