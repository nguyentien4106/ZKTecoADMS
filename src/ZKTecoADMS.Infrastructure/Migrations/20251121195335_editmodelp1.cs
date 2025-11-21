using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editmodelp1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFullDay",
                table: "Leaves",
                newName: "IsHalfShift");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Leaves",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Leaves",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Leaves");

            migrationBuilder.RenameColumn(
                name: "IsHalfShift",
                table: "Leaves",
                newName: "IsFullDay");
        }
    }
}
