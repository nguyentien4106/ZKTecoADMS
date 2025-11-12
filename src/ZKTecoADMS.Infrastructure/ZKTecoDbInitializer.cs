using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ZKTecoADMS.Infrastructure;

public class ZKTecoDbInitializer(
    ZKTecoDbContext context,
    ILogger<ZKTecoDbInitializer> logger,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager
)
{
    // Known GUIDs from init_data.sql for consistency
    private readonly Guid _adminUserId = Guid.Parse("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d");
    private readonly Guid _mainEntranceDeviceId = Guid.Parse("b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e");
    private readonly Guid _officeFloorDeviceId = Guid.Parse("c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f");
    private readonly Guid _warehouseDeviceId = Guid.Parse("d3e4f5a6-b7c8-4d9e-0f1a-2b3c4d5e6f7a");

    public async Task InitialiseAsync()
    {
        try
        {
            if (context.Database.IsNpgsql())
            {
                // Check if there are any pending migrations
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Applying {Count} pending migrations...", pendingMigrations.Count());
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Migrations applied successfully.");
                }
                else
                {
                    logger.LogInformation("Database is up to date. No pending migrations.");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
            await SeedDevicesAsync();
            await SeedUserDevicesAsync();
            await SeedAttendancesAsync();
            await SeedSystemConfigurationsAsync();
            await SeedDeviceSettingsAsync();
            await SeedSyncLogsAsync();

            await context.SaveChangesAsync();
            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    #region Seed Roles
    
    private async Task SeedRolesAsync()
    {
        var roles = new[] { nameof(Roles.Admin), nameof(Roles.User) };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole<Guid>(roleName);
                await roleManager.CreateAsync(role);
                logger.LogInformation("Created role: {RoleName}", roleName);
            }
        }
    }

    #endregion

    #region Seed Admin User
    
    private async Task SeedAdminUserAsync()
    {
        var adminEmail = "admin@zkteco.com";
        
        if (await userManager.FindByEmailAsync(adminEmail) != null)
        {
            logger.LogInformation("Admin user already exists.");
            return;
        }

        var administrator = new ApplicationUser
        {
            Id = _adminUserId,
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "System",
            LastName = "Administrator",
            EmailConfirmed = true,
            PhoneNumber = "+1234567890",
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            Created = DateTime.UtcNow,
            CreatedBy = "System"
        };

        var result = await userManager.CreateAsync(administrator, "Admin@123");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(administrator, nameof(Roles.Admin));
            logger.LogInformation("Created admin user: {Email}", adminEmail);
        }
        else
        {
            logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    #endregion

    #region Seed Devices
    
    private async Task SeedDevicesAsync()
    {
        if (await context.Devices.AnyAsync())
        {
            logger.LogInformation("Devices already exist.");
            return;
        }

        var devices = new[]
        {
            new Device
            {
                Id = _mainEntranceDeviceId,
                SerialNumber = "ZK001234567890",
                DeviceName = "Main Entrance Device",
                IpAddress = "192.168.1.100",
                Location = "Main Building - Ground Floor Entrance",
                LastOnline = DateTime.UtcNow.AddMinutes(-5),
                DeviceStatus = "Online",
                ApplicationUserId = _adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            },
            new Device
            {
                Id = _officeFloorDeviceId,
                SerialNumber = "ZK001234567891",
                DeviceName = "Office Floor 2 Device",
                IpAddress = "192.168.1.101",
                Location = "Main Building - 2nd Floor Office Area",
                LastOnline = DateTime.UtcNow.AddMinutes(-10),
                DeviceStatus = "Online",
                ApplicationUserId = _adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            },
            new Device
            {
                Id = _warehouseDeviceId,
                SerialNumber = "ZK001234567892",
                DeviceName = "Warehouse Entrance",
                IpAddress = "192.168.1.102",
                Location = "Warehouse Building - Main Gate",
                LastOnline = DateTime.UtcNow.AddHours(-1),
                DeviceStatus = "Offline",
                ApplicationUserId = _adminUserId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            }
        };

        await context.Devices.AddRangeAsync(devices);
        logger.LogInformation("Seeded {Count} devices.", devices.Length);
    }

    #endregion

    #region Seed User Devices
    
    private async Task SeedUserDevicesAsync()
    {
        if (await context.UserDevices.AnyAsync())
        {
            logger.LogInformation("User devices already exist.");
            return;
        }

        var userDevices = new[]
        {
            new User
            {
                Id = Guid.Parse("e4f5a6b7-c8d9-4e0f-1a2b-3c4d5e6f7a8b"),
                PIN = "1001",
                FullName = "John Doe",
                CardNumber = "CARD001",
                GroupId = 1,
                Privilege = 0,
                VerifyMode = (int)VerifyModes.PIN,
                IsActive = true,
                Email = "john.doe@company.com",
                PhoneNumber = "+1234567891",
                Department = "Engineering",
                DeviceId = _mainEntranceDeviceId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            },
            new User
            {
                Id = Guid.Parse("f5a6b7c8-d9e0-4f1a-2b3c-4d5e6f7a8b9c"),
                PIN = "1002",
                FullName = "Jane Smith",
                CardNumber = "CARD002",
                GroupId = 1,
                Privilege = 0,
                VerifyMode = (int)VerifyModes.PIN,
                IsActive = true,
                Email = "jane.smith@company.com",
                PhoneNumber = "+1234567892",
                Department = "Human Resources",
                DeviceId = _mainEntranceDeviceId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            },
            new User
            {
                Id = Guid.Parse("a6b7c8d9-e0f1-4a2b-3c4d-5e6f7a8b9c0d"),
                PIN = "1003",
                FullName = "Robert Johnson",
                CardNumber = "CARD003",
                GroupId = 1,
                Privilege = 2,
                VerifyMode = (int)VerifyModes.PIN,
                IsActive = true,
                Email = "robert.johnson@company.com",
                PhoneNumber = "+1234567893",
                Department = "Management",
                DeviceId = _officeFloorDeviceId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            },
            new User
            {
                Id = Guid.Parse("b7c8d9e0-f1a2-4b3c-4d5e-6f7a8b9c0d1e"),
                PIN = "1004",
                FullName = "Emily Davis",
                CardNumber = "CARD004",
                GroupId = 1,
                Privilege = 0,
                VerifyMode = (int)VerifyModes.PIN,
                IsActive = true,
                Email = "emily.davis@company.com",
                PhoneNumber = "+1234567894",
                Department = "Marketing",
                DeviceId = _officeFloorDeviceId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            },
            new User
            {
                Id = Guid.Parse("c8d9e0f1-a2b3-4c4d-5e6f-7a8b9c0d1e2f"),
                PIN = "1005",
                FullName = "Michael Brown",
                CardNumber = "CARD005",
                GroupId = 1,
                Privilege = 0,
                VerifyMode = (int)VerifyModes.PIN,
                IsActive = true,
                Email = "michael.brown@company.com",
                PhoneNumber = "+1234567895",
                Department = "Operations",
                DeviceId = _warehouseDeviceId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            },
            new User
            {
                Id = Guid.Parse("d9e0f1a2-b3c4-4d5e-6f7a-8b9c0d1e2f3a"),
                PIN = "1006",
                FullName = "Sarah Wilson",
                CardNumber = "CARD006",
                GroupId = 1,
                Privilege = 0,
                VerifyMode = (int)VerifyModes.PIN,
                IsActive = true,
                Email = "sarah.wilson@company.com",
                PhoneNumber = "+1234567896",
                Department = "Finance",
                DeviceId = _mainEntranceDeviceId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            },
            new User
            {
                Id = Guid.Parse("e0f1a2b3-c4d5-4e6f-7a8b-9c0d1e2f3a4b"),
                PIN = "1007",
                FullName = "David Martinez",
                CardNumber = "CARD007",
                GroupId = 1,
                Privilege = 0,
                VerifyMode = (int)VerifyModes.PIN,
                IsActive = true,
                Email = "david.martinez@company.com",
                PhoneNumber = "+1234567897",
                Department = "IT Support",
                DeviceId = _officeFloorDeviceId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            },
            new User
            {
                Id = Guid.Parse("f1a2b3c4-d5e6-4f7a-8b9c-0d1e2f3a4b5c"),
                PIN = "1008",
                FullName = "Lisa Anderson",
                CardNumber = "CARD008",
                GroupId = 1,
                Privilege = 0,
                VerifyMode = (int)VerifyModes.PIN,
                IsActive = false,
                Email = "lisa.anderson@company.com",
                PhoneNumber = "+1234567898",
                Department = "Sales",
                DeviceId = _warehouseDeviceId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin@zkteco.com"
            }
        };

        await context.UserDevices.AddRangeAsync(userDevices);
        logger.LogInformation("Seeded {Count} user devices.", userDevices.Length);
    }

    #endregion

    #region Seed Attendance Logs
    
    private async Task SeedAttendancesAsync()
    {
        if (await context.AttendanceLogs.AnyAsync())
        {
            logger.LogInformation("Attendance logs already exist.");
            return;
        }

        var Attendances = new[]
        {
            // Today's check-ins
            new Attendance
            {
                Id = Guid.Parse("a1a1a1a1-b1b1-4c1c-8d1d-1e1e1e1e1e1e"),
                DeviceId = _mainEntranceDeviceId,
                UserId = Guid.Parse("e4f5a6b7-c8d9-4e0f-1a2b-3c4d5e6f7a8b"),
                PIN = "1001",
                VerifyMode = VerifyModes.Password,
                AttendanceState = 0,
                AttendanceTime = DateTime.UtcNow.AddHours(-8),
                CreatedAt = DateTime.UtcNow.AddHours(-8),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("a2a2a2a2-b2b2-4c2c-8d2d-2e2e2e2e2e2e"),
                DeviceId = _mainEntranceDeviceId,
                UserId = Guid.Parse("f5a6b7c8-d9e0-4f1a-2b3c-4d5e6f7a8b9c"),
                PIN = "1002",
                VerifyMode = VerifyModes.FingerAndPassword,
                AttendanceState = 0,
                AttendanceTime = DateTime.UtcNow.AddHours(-7).AddMinutes(-45),
                CreatedAt = DateTime.UtcNow.AddHours(-7).AddMinutes(-45),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("a3a3a3a3-b3b3-4c3c-8d3d-3e3e3e3e3e3e"),
                DeviceId = _officeFloorDeviceId,
                UserId = Guid.Parse("a6b7c8d9-e0f1-4a2b-3c4d-5e6f7a8b9c0d"),
                PIN = "1003",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = 0,
                AttendanceTime = DateTime.UtcNow.AddHours(-7).AddMinutes(-30),
                CreatedAt = DateTime.UtcNow.AddHours(-7).AddMinutes(-30),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("a4a4a4a4-b4b4-4c4c-8d4d-4e4e4e4e4e4e"),
                DeviceId = _officeFloorDeviceId,
                UserId = Guid.Parse("b7c8d9e0-f1a2-4b3c-4d5e-6f7a8b9c0d1e"),
                PIN = "1004",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = 0,
                AttendanceTime = DateTime.UtcNow.AddHours(-8).AddMinutes(-15),
                CreatedAt = DateTime.UtcNow.AddHours(-8).AddMinutes(-15),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("a5a5a5a5-b5b5-4c5c-8d5d-5e5e5e5e5e5e"),
                DeviceId = _warehouseDeviceId,
                UserId = Guid.Parse("c8d9e0f1-a2b3-4c4d-5e6f-7a8b9c0d1e2f"),
                PIN = "1005",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = 0,
                AttendanceTime = DateTime.UtcNow.AddHours(-8).AddMinutes(-5),
                CreatedAt = DateTime.UtcNow.AddHours(-8).AddMinutes(-5),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("a6a6a6a6-b6b6-4c6c-8d6d-6e6e6e6e6e6e"),
                DeviceId = _mainEntranceDeviceId,
                UserId = Guid.Parse("d9e0f1a2-b3c4-4d5e-6f7a-8b9c0d1e2f3a"),
                PIN = "1006",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = 0,
                AttendanceTime = DateTime.UtcNow.AddHours(-7).AddMinutes(-50),
                CreatedAt = DateTime.UtcNow.AddHours(-7).AddMinutes(-50),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("a7a7a7a7-b7b7-4c7c-8d7d-7e7e7e7e7e7e"),
                DeviceId = _officeFloorDeviceId,
                UserId = Guid.Parse("e0f1a2b3-c4d5-4e6f-7a8b-9c0d1e2f3a4b"),
                PIN = "1007",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = 0,
                AttendanceTime = DateTime.UtcNow.AddHours(-8).AddMinutes(-10),
                CreatedAt = DateTime.UtcNow.AddHours(-8).AddMinutes(-10),
                CreatedBy = "System"
            },
            // Today's check-outs
            new Attendance
            {
                Id = Guid.Parse("a8a8a8a8-b8b8-4c8c-8d8d-8e8e8e8e8e8e"),
                DeviceId = _mainEntranceDeviceId,
                UserId = Guid.Parse("e4f5a6b7-c8d9-4e0f-1a2b-3c4d5e6f7a8b"),
                PIN = "1001",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = AttendanceStates.CheckIn,
                AttendanceTime = DateTime.UtcNow.AddMinutes(-30),
                CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("a9a9a9a9-b9b9-4c9c-8d9d-9e9e9e9e9e9e"),
                DeviceId = _mainEntranceDeviceId,
                UserId = Guid.Parse("f5a6b7c8-d9e0-4f1a-2b3c-4d5e6f7a8b9c"),
                PIN = "1002",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = AttendanceStates.CheckOut,
                AttendanceTime = DateTime.UtcNow.AddMinutes(-45),
                CreatedAt = DateTime.UtcNow.AddMinutes(-45),
                CreatedBy = "System"
            },
            // Yesterday's attendance
            new Attendance
            {
                Id = Guid.Parse("b1b1b1b1-c1c1-4d1d-8e1e-1f1f1f1f1f1f"),
                DeviceId = _mainEntranceDeviceId,
                UserId = Guid.Parse("e4f5a6b7-c8d9-4e0f-1a2b-3c4d5e6f7a8b"),
                PIN = "1001",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = AttendanceStates.CheckIn,
                AttendanceTime = DateTime.UtcNow.AddDays(-1).AddHours(-8),
                CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(-8),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("b2b2b2b2-c2c2-4d2d-8e2e-2f2f2f2f2f2f"),
                DeviceId = _mainEntranceDeviceId,
                UserId = Guid.Parse("f5a6b7c8-d9e0-4f1a-2b3c-4d5e6f7a8b9c"),
                PIN = "1002",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = AttendanceStates.CheckOut,
                AttendanceTime = DateTime.UtcNow.AddDays(-1).AddHours(-7).AddMinutes(-55),
                CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(-7).AddMinutes(-55),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("b3b3b3b3-c3c3-4d3d-8e3e-3f3f3f3f3f3f"),
                DeviceId = _officeFloorDeviceId,
                UserId = Guid.Parse("a6b7c8d9-e0f1-4a2b-3c4d-5e6f7a8b9c0d"),
                PIN = "1003",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = 0,
                AttendanceTime = DateTime.UtcNow.AddDays(-1).AddHours(-7).AddMinutes(-40),
                CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(-7).AddMinutes(-40),
                CreatedBy = "System"
            },
            new Attendance
            {
                Id = Guid.Parse("b4b4b4b4-c4c4-4d4d-8e4e-4f4f4f4f4f4f"),
                DeviceId = _officeFloorDeviceId,
                UserId = Guid.Parse("b7c8d9e0-f1a2-4b3c-4d5e-6f7a8b9c0d1e"),
                PIN = "1004",
                VerifyMode = VerifyModes.PIN,
                AttendanceState = 0,
                AttendanceTime = DateTime.UtcNow.AddDays(-1).AddHours(-8).AddMinutes(-20),
                CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(-8).AddMinutes(-20),
                CreatedBy = "System"
            }
        };

        await context.AttendanceLogs.AddRangeAsync(Attendances);
        logger.LogInformation("Seeded {Count} attendance logs.", Attendances.Length);
    }

    #endregion

    #region Seed System Configurations
    
    private async Task SeedSystemConfigurationsAsync()
    {
        if (await context.SystemConfigurations.AnyAsync())
        {
            logger.LogInformation("System configurations already exist.");
            return;
        }

        var configurations = new[]
        {
            new SystemConfiguration
            {
                Id = Guid.Parse("11111111-2222-3333-4444-555555555551"),
                ConfigKey = "SyncInterval",
                ConfigValue = "300",
                Description = "Attendance sync interval in seconds (5 minutes)",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new SystemConfiguration
            {
                Id = Guid.Parse("11111111-2222-3333-4444-555555555552"),
                ConfigKey = "MaxRetryAttempts",
                ConfigValue = "3",
                Description = "Maximum retry attempts for failed device connections",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new SystemConfiguration
            {
                Id = Guid.Parse("11111111-2222-3333-4444-555555555553"),
                ConfigKey = "SessionTimeout",
                ConfigValue = "30",
                Description = "User session timeout in minutes",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new SystemConfiguration
            {
                Id = Guid.Parse("11111111-2222-3333-4444-555555555554"),
                ConfigKey = "EnablePushNotifications",
                ConfigValue = "true",
                Description = "Enable push notifications for attendance events",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new SystemConfiguration
            {
                Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                ConfigKey = "DefaultTimezone",
                ConfigValue = "UTC",
                Description = "Default system timezone",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new SystemConfiguration
            {
                Id = Guid.Parse("11111111-2222-3333-4444-555555555556"),
                ConfigKey = "MaxDevicesPerUser",
                ConfigValue = "10",
                Description = "Maximum number of devices per user account",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            }
        };

        await context.SystemConfigurations.AddRangeAsync(configurations);
        logger.LogInformation("Seeded {Count} system configurations.", configurations.Length);
    }

    #endregion

    #region Seed Device Settings
    
    private async Task SeedDeviceSettingsAsync()
    {
        if (await context.DeviceSettings.AnyAsync())
        {
            logger.LogInformation("Device settings already exist.");
            return;
        }

        var deviceSettings = new[]
        {
            // Main Entrance Device Settings
            new DeviceSetting
            {
                Id = Guid.Parse("22222222-3333-4444-5555-666666666661"),
                DeviceId = _mainEntranceDeviceId,
                SettingKey = "SleepTime",
                SettingValue = "5",
                Description = "Device sleep time in minutes",
                CreatedAt = DateTime.UtcNow
            },
            new DeviceSetting
            {
                Id = Guid.Parse("22222222-3333-4444-5555-666666666662"),
                DeviceId = _mainEntranceDeviceId,
                SettingKey = "Volume",
                SettingValue = "50",
                Description = "Device volume level (0-100)",
                CreatedAt = DateTime.UtcNow
            },
            // Office Floor Device Settings
            new DeviceSetting
            {
                Id = Guid.Parse("22222222-3333-4444-5555-666666666663"),
                DeviceId = _officeFloorDeviceId,
                SettingKey = "SleepTime",
                SettingValue = "10",
                Description = "Device sleep time in minutes",
                CreatedAt = DateTime.UtcNow
            },
            new DeviceSetting
            {
                Id = Guid.Parse("22222222-3333-4444-5555-666666666664"),
                DeviceId = _officeFloorDeviceId,
                SettingKey = "Volume",
                SettingValue = "60",
                Description = "Device volume level (0-100)",
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.DeviceSettings.AddRangeAsync(deviceSettings);
        logger.LogInformation("Seeded {Count} device settings.", deviceSettings.Length);
    }

    #endregion

    #region Seed Sync Logs
    
    private async Task SeedSyncLogsAsync()
    {
        // if (await context.SyncLogs.AnyAsync())
        // {
        //     logger.LogInformation("Sync logs already exist.");
        //     return;
        // }

        // var syncLogs = new[]
        // {
        //     new SyncLog
        //     {
        //         Id = Guid.Parse("33333333-4444-5555-6666-777777777771"),
        //         DeviceId = _mainEntranceDeviceId,
        //         SyncType = "Attendance",
        //         SyncStatus = "Success",
        //         RecordsProcessed = 45,
        //         ErrorMessage = null,
        //         StartTime = DateTime.UtcNow.AddHours(-1),
        //         EndTime = DateTime.UtcNow.AddMinutes(-59)
        //     },
        //     new SyncLog
        //     {
        //         Id = Guid.Parse("33333333-4444-5555-6666-777777777772"),
        //         DeviceId = _officeFloorDeviceId,
        //         SyncType = "Attendance",
        //         SyncStatus = "Success",
        //         RecordsProcessed = 32,
        //         ErrorMessage = null,
        //         StartTime = DateTime.UtcNow.AddHours(-2),
        //         EndTime = DateTime.UtcNow.AddHours(-1).AddMinutes(-58)
        //     },
        //     new SyncLog
        //     {
        //         Id = Guid.Parse("33333333-4444-5555-6666-777777777773"),
        //         DeviceId = _warehouseDeviceId,
        //         SyncType = "Attendance",
        //         SyncStatus = "Failed",
        //         RecordsProcessed = 0,
        //         ErrorMessage = "Connection timeout - device offline",
        //         StartTime = DateTime.UtcNow.AddHours(-3),
        //         EndTime = DateTime.UtcNow.AddHours(-2).AddMinutes(-59)
        //     }
        // };

        // await context.SyncLogs.AddRangeAsync(syncLogs);
        // logger.LogInformation("Seeded {Count} sync logs.", syncLogs.Length);
    }

    #endregion
}
