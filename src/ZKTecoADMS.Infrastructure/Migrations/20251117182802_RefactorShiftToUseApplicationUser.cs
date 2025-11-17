using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorShiftToUseApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Employees_EmployeeId",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Shifts",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_EmployeeId",
                table: "Shifts",
                newName: "IX_Shifts_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_AspNetUsers_ApplicationUserId",
                table: "Shifts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_AspNetUsers_ApplicationUserId",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Shifts",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_ApplicationUserId",
                table: "Shifts",
                newName: "IX_Shifts_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Employees_EmployeeId",
                table: "Shifts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
