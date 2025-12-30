using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.HasIndex(s => s.EmployeeId);
        
        // Relationship: ApplicationUser -> RequestedShifts
        //
        // // Relationship: Shift -> CheckInAttendance
        // builder.HasOne(s => s.CheckInAttendance)
        //     .WithMany()
        //     .HasForeignKey(s => s.CheckInAttendanceId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // // Relationship: Shift -> CheckOutAttendance
        // builder.HasOne(s => s.CheckOutAttendance)
        //     .WithMany()
        //     .HasForeignKey(s => s.CheckOutAttendanceId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}
