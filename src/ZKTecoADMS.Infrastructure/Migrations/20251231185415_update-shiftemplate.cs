using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateshiftemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaximumAllowedEarlyLeaveMinutes",
                table: "ShiftTemplates");

            migrationBuilder.DropColumn(
                name: "MaximumAllowedLateMinutes",
                table: "ShiftTemplates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaximumAllowedEarlyLeaveMinutes",
                table: "ShiftTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaximumAllowedLateMinutes",
                table: "ShiftTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
