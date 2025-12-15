using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthInsuranceFieldsToSalaryProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasHealthInsurance",
                table: "SalaryProfiles",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HealthInsuranceRate",
                table: "SalaryProfiles",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasHealthInsurance",
                table: "SalaryProfiles");

            migrationBuilder.DropColumn(
                name: "HealthInsuranceRate",
                table: "SalaryProfiles");
        }
    }
}
