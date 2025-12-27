using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatesalaryprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AttendanceBonus",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckIn",
                table: "EmployeeSalaryProfiles",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckOut",
                table: "EmployeeSalaryProfiles",
                type: "time without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "EmployeeSalaryProfiles",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasHealthInsurance",
                table: "EmployeeSalaryProfiles",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HealthInsuranceRate",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HolidayMultiplier",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HousingAllowance",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MealAllowance",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NightShiftMultiplier",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NightShiftRate",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OTRateHoliday",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OTRateWeekday",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OTRateWeekend",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OvertimeMultiplier",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidLeaveDays",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PhoneSkillShiftAllowance",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RateType",
                table: "EmployeeSalaryProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ResponsibilityAllowance",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryPerDay",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StandardHoursPerDay",
                table: "EmployeeSalaryProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransportAllowance",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnpaidLeaveDays",
                table: "EmployeeSalaryProfiles",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeeklyOffDays",
                table: "EmployeeSalaryProfiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceBonus",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "HasHealthInsurance",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "HealthInsuranceRate",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "HolidayMultiplier",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "HousingAllowance",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "MealAllowance",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "NightShiftMultiplier",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "NightShiftRate",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OTRateHoliday",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OTRateWeekday",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OTRateWeekend",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "OvertimeMultiplier",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "PaidLeaveDays",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "PhoneSkillShiftAllowance",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "RateType",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "ResponsibilityAllowance",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "SalaryPerDay",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "StandardHoursPerDay",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "TransportAllowance",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "UnpaidLeaveDays",
                table: "EmployeeSalaryProfiles");

            migrationBuilder.DropColumn(
                name: "WeeklyOffDays",
                table: "EmployeeSalaryProfiles");
        }
    }
}
