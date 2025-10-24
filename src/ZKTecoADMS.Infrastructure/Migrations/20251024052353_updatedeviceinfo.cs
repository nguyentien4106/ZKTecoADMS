using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedeviceinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceInfos_DeviceInfoId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceInfoId",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceInfos_DeviceId",
                table: "DeviceInfos",
                column: "DeviceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceInfos_Devices_DeviceId",
                table: "DeviceInfos",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceInfos_Devices_DeviceId",
                table: "DeviceInfos");

            migrationBuilder.DropIndex(
                name: "IX_DeviceInfos_DeviceId",
                table: "DeviceInfos");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceInfoId",
                table: "Devices",
                column: "DeviceInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceInfos_DeviceInfoId",
                table: "Devices",
                column: "DeviceInfoId",
                principalTable: "DeviceInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
