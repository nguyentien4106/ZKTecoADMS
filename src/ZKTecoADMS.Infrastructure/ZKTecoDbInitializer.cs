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
        var userEmail = role.ToString() + "@gmail.com";

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
            LastName = "" + role.ToString().ToUpperInvariant(),
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

}
