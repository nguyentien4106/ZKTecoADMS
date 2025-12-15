using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExtendedFieldsToSalaryProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AttendanceBonus",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FixedWeeklyOffDays",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HealthInsurance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HousingAllowance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LateToleranceMinutes",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MealAllowance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NightShiftRate",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OTHourLimitPerMonth",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OTRateHoliday",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OTRateWeekday",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OTRateWeekend",
                table: "SalaryProfiles",
                type: "numeric",
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

            migrationBuilder.AddColumn<int>(
                name: "PaidLeaveDays",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PersonalIncomeTax",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PhoneSkillShiftAllowance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ResponsibilityAllowance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryPerDay",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryPerHour",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SocialInsurance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StandardHoursPerDay",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StandardWorkingDaysPerMonth",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransportAllowance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnemploymentInsurance",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnpaidLeaveDays",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceBonus",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "FixedWeeklyOffDays",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "HealthInsurance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "HousingAllowance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "LateToleranceMinutes",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "MealAllowance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "NightShiftRate",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OTHourLimitPerMonth",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OTRateHoliday",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OTRateWeekday",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OTRateWeekend",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OfficialHolidays",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OtherDeductionsPenalties",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "PaidLeaveDays",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "PersonalIncomeTax",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "PhoneSkillShiftAllowance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "ResponsibilityAllowance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "SalaryPerDay",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "SalaryPerHour",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "SocialInsurance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "StandardHoursPerDay",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "StandardWorkingDaysPerMonth",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "TransportAllowance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "UnemploymentInsurance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "UnpaidLeaveDays",
                table: "SalaryProfiles");
        }
    }
}
