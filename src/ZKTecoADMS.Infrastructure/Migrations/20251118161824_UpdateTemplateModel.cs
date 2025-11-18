using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZKTecoADMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTemplateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert StartTime from timestamp to interval using USING clause
            migrationBuilder.Sql(@"
                ALTER TABLE ""ShiftTemplates"" 
                ALTER COLUMN ""StartTime"" TYPE interval 
                USING (""StartTime""::time)::interval;
            ");

            // Convert EndTime from timestamp to interval using USING clause
            migrationBuilder.Sql(@"
                ALTER TABLE ""ShiftTemplates"" 
                ALTER COLUMN ""EndTime"" TYPE interval 
                USING (""EndTime""::time)::interval;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert EndTime from interval back to timestamp
            migrationBuilder.Sql(@"
                ALTER TABLE ""ShiftTemplates"" 
                ALTER COLUMN ""EndTime"" TYPE timestamp without time zone 
                USING (CURRENT_DATE + ""EndTime"");
            ");

            // Revert StartTime from interval back to timestamp
            migrationBuilder.Sql(@"
                ALTER TABLE ""ShiftTemplates"" 
                ALTER COLUMN ""StartTime"" TYPE timestamp without time zone 
                USING (CURRENT_DATE + ""StartTime"");
            ");
        }
    }
}
