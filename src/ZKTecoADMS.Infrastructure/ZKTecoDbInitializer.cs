using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ZKTecoADMS.Infrastructure;

public class ZKTecoDbInitializer(
    ZKTecoDbContext context,
    ILogger<ZKTecoDbInitializer> logger,

    UserManager<ApplicationUser> userManager,

    RoleManager<IdentityRole<Guid>> roleManager
    
)
{
    public async Task InitialiseAsync()
    {
        try
        {
            if (context.Database.IsNpgsql())
            {
                await context.Database.MigrateAsync();
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
            await TrySeedUsersAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedUsersAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole<Guid>(nameof(Roles.Admin));

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            var role = await roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "Admin", Email = "admin@gmail.com", FirstName = "Admin", LastName = "Account",
            EmailConfirmed = true };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Ti100600@");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Default user role
        var userRole = new IdentityRole<Guid>(nameof(Roles.User));

        if (roleManager.Roles.All(r => r.Name != userRole.Name))
        {
            var role = await roleManager.CreateAsync(userRole);
        }

        // Default users
        var user = new ApplicationUser { UserName = "User", Email = "user@gmail.com", FirstName = "User", LastName = "Account",
            EmailConfirmed = true };

        if (userManager.Users.All(u => u.UserName != user.UserName))
        {
            await userManager.CreateAsync(user, "Ti100600@");
            if (!string.IsNullOrWhiteSpace(userRole.Name))
            {
                await userManager.AddToRolesAsync(user, new[] { userRole.Name });
            }
        }
    }

}
