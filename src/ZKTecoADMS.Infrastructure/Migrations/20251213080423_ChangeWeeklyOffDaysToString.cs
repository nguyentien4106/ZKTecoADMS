using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWeeklyOffDaysToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedWeeklyOffDays",
                table: "SalaryProfiles");

            migrationBuilder.AddColumn<string>(
                name: "WeeklyOffDays",
                table: "SalaryProfiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeeklyOffDays",
                table: "SalaryProfiles");

            migrationBuilder.AddColumn<int>(
                name: "FixedWeeklyOffDays",
                table: "SalaryProfiles",
                type: "integer",
                nullable: true);
        }
    }
}
