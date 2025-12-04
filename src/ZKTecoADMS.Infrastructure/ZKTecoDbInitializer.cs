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
            await SeedUsersAsync();
            await SeedShiftTemplatesAsync();

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
        var roles = new[] { nameof(Roles.Admin), nameof(Roles.User), nameof(Roles.Manager), nameof(Roles.Employee) };

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

    private async Task SeedUsersAsync()
    {
        await SeedUserAsync(Roles.Admin);
        await SeedUserAsync(Roles.Manager);
        await SeedUserAsync(Roles.Employee);
        await SeedUserAsync(Roles.User);
    }

    private async Task SeedUserAsync(Roles role)
    {
        var userEmail = role.ToString().ToLower() + "@gmail.com";

        if (await userManager.FindByEmailAsync(userEmail) != null)
        {
            logger.LogInformation("User already exists.");
            return;
        }

        var user = new ApplicationUser
        {
            UserName = userEmail.Split("@")[0],
            Email = userEmail,
            FirstName = "System",
            LastName = "" + role.ToString(),
            EmailConfirmed = true,
            PhoneNumber = "+1234567890",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = true,
            LockoutEnabled = true,
            AccessFailedCount = 0,
            Created = DateTime.Now,
            CreatedBy = "System"
        };

        var result = await userManager.CreateAsync(user, "Ti100600@");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role.ToString());
            logger.LogInformation("Created user: {Email}", userEmail);
        }
        else
        {
            logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    #region Seed Shift Templates

    private async Task SeedShiftTemplatesAsync()
    {
        // Check if shift templates already exist
        if (await context.ShiftTemplates.AnyAsync())
        {
            logger.LogInformation("Shift templates already exist. Skipping seed.");
            return;
        }

        // Get the manager user to assign templates to
        var managerEmail = Roles.Manager.ToString().ToLower() + "@gmail.com";
        var manager = await userManager.FindByEmailAsync(managerEmail);

        if (manager == null)
        {
            logger.LogWarning("Manager user not found. Cannot seed shift templates.");
            return;
        }

        var shiftTemplates = new List<ShiftTemplate>
        {
            new ShiftTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Morning Shift (8:00 - 17:00)",
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(17, 0, 0),
                MaximumAllowedLateMinutes = 30,
                MaximumAllowedEarlyLeaveMinutes = 30,
                IsActive = true,
                ManagerId = manager.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new ShiftTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Standard Shift (9:00 - 18:00)",
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(18, 0, 0),
                MaximumAllowedLateMinutes = 30,
                MaximumAllowedEarlyLeaveMinutes = 30,
                IsActive = true,
                ManagerId = manager.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new ShiftTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Late Morning Shift (10:00 - 19:00)",
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(19, 0, 0),
                MaximumAllowedLateMinutes = 30,
                MaximumAllowedEarlyLeaveMinutes = 30,
                IsActive = true,
                ManagerId = manager.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            }
        };

        await context.ShiftTemplates.AddRangeAsync(shiftTemplates);
        logger.LogInformation("Created {Count} shift templates", shiftTemplates.Count);
    }

    #endregion

}
