using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShiftTimeToleranceProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaximumAllowedEarlyLeaveMinutes",
                table: "Shifts",
                type: "integer",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.AddColumn<int>(
                name: "MaximumAllowedLateMinutes",
                table: "Shifts",
                type: "integer",
                nullable: false,
                defaultValue: 30);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumAllowedEarlyLeaveMinutes",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "MaximumAllowedLateMinutes",
                table: "Shifts");
        }
    }
}
