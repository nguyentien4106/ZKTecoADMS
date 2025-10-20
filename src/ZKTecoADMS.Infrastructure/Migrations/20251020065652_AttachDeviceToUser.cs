using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AttachDeviceToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Devices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ApplicationUserId",
                table: "Devices",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_AspNetUsers_ApplicationUserId",
                table: "Devices",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_AspNetUsers_ApplicationUserId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_ApplicationUserId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Devices");
        }
    }
}
