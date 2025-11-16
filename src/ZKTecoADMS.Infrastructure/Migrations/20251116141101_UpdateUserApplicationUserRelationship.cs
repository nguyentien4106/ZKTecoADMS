using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserApplicationUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "UserDevices",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_ApplicationUserId",
                table: "UserDevices",
                column: "ApplicationUserId",
                unique: true,
                filter: "\"ApplicationUserId\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDevices_AspNetUsers_ApplicationUserId",
                table: "UserDevices",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDevices_AspNetUsers_ApplicationUserId",
                table: "UserDevices");

            migrationBuilder.DropIndex(
                name: "IX_UserDevices_ApplicationUserId",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserDevices");
        }
    }
}
