using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ZKTecoADMS.Infrastructure;

public class ZKTecoDbContext(DbContextOptions<ZKTecoDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<FingerprintTemplate> FingerprintTemplates => Set<FingerprintTemplate>();
    public DbSet<FaceTemplate> FaceTemplates => Set<FaceTemplate>();
    public DbSet<Attendance> AttendanceLogs => Set<Attendance>();
    public DbSet<DeviceCommand> DeviceCommands => Set<DeviceCommand>();
    public DbSet<SyncLog> SyncLogs => Set<SyncLog>();
    public DbSet<DeviceSetting> DeviceSettings => Set<DeviceSetting>();
    public DbSet<SystemConfiguration> SystemConfigurations => Set<SystemConfiguration>();
    public DbSet<DeviceInfo> DeviceInfos => Set<DeviceInfo>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<ShiftTemplate> ShiftTemplates => Set<ShiftTemplate>();
    public DbSet<Leave> Leaves => Set<Leave>();
    public DbSet<SalaryProfile> SalaryProfiles => Set<SalaryProfile>();
    public DbSet<EmployeeSalaryProfile> EmployeeSalaryProfiles => Set<EmployeeSalaryProfile>();
    public DbSet<Payslip> Payslips => Set<Payslip>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    public DbSet<EmployeeWorkingInfo> EmployeeWorkingInfos => Set<EmployeeWorkingInfo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        // Configure all DateTime properties to use timestamp without time zone
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetColumnType("timestamp without time zone");
                }
            }
        }
    }

}
