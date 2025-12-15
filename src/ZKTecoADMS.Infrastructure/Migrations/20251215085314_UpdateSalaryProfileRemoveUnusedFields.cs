using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalaryProfileRemoveUnusedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HealthInsurance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "LateToleranceMinutes",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OTHourLimitPerMonth",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OfficialHolidays",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OtherDeductionsPenalties",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "PersonalIncomeTax",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "SocialInsurance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "StandardWorkingDaysPerMonth",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "UnemploymentInsurance",
                table: "SalaryProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HealthInsurance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LateToleranceMinutes",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OTHourLimitPerMonth",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OfficialHolidays",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherDeductionsPenalties",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PersonalIncomeTax",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SocialInsurance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StandardWorkingDaysPerMonth",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnemploymentInsurance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);
        }
    }
}
