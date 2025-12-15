using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeWorkingInfoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeWorkingInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BalancedLeaveDays = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalLeaveDays = table.Column<decimal>(type: "numeric", nullable: false),
                    BalancedLateEarlyLeaveMinutes = table.Column<decimal>(type: "numeric", nullable: false),
                    StandardHoursPerDay = table.Column<int>(type: "integer", nullable: true),
                    WeeklyOffDays = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PaidLeaveDaysPerYear = table.Column<int>(type: "integer", nullable: true),
                    UnpaidLeaveDaysPerYear = table.Column<int>(type: "integer", nullable: true),
                    OfficialHolidaysPerYear = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkingInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkingInfos_AspNetUsers_EmployeeUserId",
                        column: x => x.EmployeeUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkingInfos_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkingInfos_EmployeeId",
                table: "EmployeeWorkingInfos",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkingInfos_EmployeeUserId",
                table: "EmployeeWorkingInfos",
                column: "EmployeeUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeWorkingInfos");
        }
    }
}
