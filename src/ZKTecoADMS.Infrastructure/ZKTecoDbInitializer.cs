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
    private Guid ManagerUserId = Guid.Parse("698ba485-023f-4cf8-8439-99e7d04c459a");
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
            await SeedEmployeeAsync();
            await SeedShiftTemplatesAsync();
            await SeedHolidaysAsync();

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
            Id = role == Roles.Manager ? ManagerUserId : Guid.NewGuid(),
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
            CreatedAt = DateTime.Now,
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

    private async Task SeedEmployeeAsync()
    {
        var manager = await userManager.FindByIdAsync(ManagerUserId.ToString());
        if (manager == null)
        {
            logger.LogWarning("Manager user not found. Cannot seed employees.");
            return;
        }

        var employees = new List<Employee>
        {
            new Employee
            {
                Id = Guid.NewGuid(),
                EmployeeCode = "EMP001",
                FirstName = "Nguyen Van",
                LastName = "An",
                Gender = "Male",
                DateOfBirth = new DateTime(1990, 5, 15),
                NationalIdNumber = "001234567890",
                NationalIdIssueDate = new DateTime(2015, 6, 1),
                NationalIdIssuePlace = "Ha Noi",
                PhoneNumber = "+84901234567",
                PersonalEmail = "nguyenvanan@gmail.com",
                CompanyEmail = "an.nguyen@company.com",
                PermanentAddress = "123 Nguyen Trai, Thanh Xuan, Ha Noi",
                TemporaryAddress = "123 Nguyen Trai, Thanh Xuan, Ha Noi",
                EmergencyContactName = "Nguyen Van B",
                EmergencyContactPhone = "+84902345678",
                Department = "IT",
                Position = "Senior Developer",
                Level = "Senior",
                JoinDate = new DateTime(2020, 1, 15),
                ProbationEndDate = new DateTime(2020, 3, 15),
                WorkStatus = EmployeeWorkStatus.Active,
                EmploymentType = EmploymentType.Monthly,
                ManagerId = manager.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                EmployeeCode = "EMP002",
                FirstName = "Tran Thi",
                LastName = "Binh",
                Gender = "Female",
                DateOfBirth = new DateTime(1995, 8, 20),
                NationalIdNumber = "001234567891",
                NationalIdIssueDate = new DateTime(2016, 7, 10),
                NationalIdIssuePlace = "Ho Chi Minh",
                PhoneNumber = "+84903456789",
                PersonalEmail = "tranthibinh@gmail.com",
                CompanyEmail = "binh.tran@company.com",
                PermanentAddress = "456 Le Van Viet, Thu Duc, Ho Chi Minh",
                TemporaryAddress = "456 Le Van Viet, Thu Duc, Ho Chi Minh",
                EmergencyContactName = "Tran Van C",
                EmergencyContactPhone = "+84904567890",
                Department = "HR",
                Position = "HR Manager",
                Level = "Lead",
                JoinDate = new DateTime(2019, 6, 1),
                ProbationEndDate = new DateTime(2019, 8, 1),
                WorkStatus = EmployeeWorkStatus.Active,
                EmploymentType = EmploymentType.Monthly,
                ManagerId = manager.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                EmployeeCode = "EMP003",
                FirstName = "Le Minh",
                LastName = "Chau",
                Gender = "Male",
                DateOfBirth = new DateTime(1992, 3, 10),
                NationalIdNumber = "001234567892",
                NationalIdIssueDate = new DateTime(2017, 4, 15),
                NationalIdIssuePlace = "Da Nang",
                PhoneNumber = "+84905678901",
                PersonalEmail = "leminhchau@gmail.com",
                CompanyEmail = "chau.le@company.com",
                PermanentAddress = "789 Tran Phu, Hai Chau, Da Nang",
                TemporaryAddress = "789 Tran Phu, Hai Chau, Da Nang",
                EmergencyContactName = "Le Van D",
                EmergencyContactPhone = "+84906789012",
                Department = "IT",
                Position = "Junior Developer",
                Level = "Junior",
                JoinDate = new DateTime(2022, 9, 1),
                ProbationEndDate = new DateTime(2022, 11, 1),
                WorkStatus = EmployeeWorkStatus.Active,
                EmploymentType = EmploymentType.Monthly,
                ManagerId = manager.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                EmployeeCode = "EMP004",
                FirstName = "Pham Thi",
                LastName = "Dung",
                Gender = "Female",
                DateOfBirth = new DateTime(1988, 12, 5),
                NationalIdNumber = "001234567893",
                NationalIdIssueDate = new DateTime(2014, 5, 20),
                NationalIdIssuePlace = "Ha Noi",
                PhoneNumber = "+84907890123",
                PersonalEmail = "phamthidung@gmail.com",
                CompanyEmail = "dung.pham@company.com",
                PermanentAddress = "321 Giai Phong, Dong Da, Ha Noi",
                TemporaryAddress = "321 Giai Phong, Dong Da, Ha Noi",
                EmergencyContactName = "Pham Van E",
                EmergencyContactPhone = "+84908901234",
                Department = "Finance",
                Position = "Accountant",
                Level = "Senior",
                JoinDate = new DateTime(2018, 3, 15),
                ProbationEndDate = new DateTime(2018, 5, 15),
                WorkStatus = EmployeeWorkStatus.Active,
                EmploymentType = EmploymentType.Monthly,
                ManagerId = manager.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                EmployeeCode = "EMP005",
                FirstName = "Hoang Van",
                LastName = "E",
                Gender = "Male",
                DateOfBirth = new DateTime(1993, 7, 25),
                NationalIdNumber = "001234567894",
                NationalIdIssueDate = new DateTime(2018, 8, 10),
                NationalIdIssuePlace = "Ho Chi Minh",
                PhoneNumber = "+84909012345",
                PersonalEmail = "hoangvane@gmail.com",
                CompanyEmail = "e.hoang@company.com",
                PermanentAddress = "654 Nguyen Hue, Quan 1, Ho Chi Minh",
                TemporaryAddress = "654 Nguyen Hue, Quan 1, Ho Chi Minh",
                EmergencyContactName = "Hoang Thi F",
                EmergencyContactPhone = "+84900123456",
                Department = "Sales",
                Position = "Sales Executive",
                Level = "Junior",
                JoinDate = new DateTime(2021, 11, 1),
                ProbationEndDate = new DateTime(2022, 1, 1),
                WorkStatus = EmployeeWorkStatus.Active,
                EmploymentType = EmploymentType.Hourly,
                ManagerId = manager.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            }
        };

        var currentEmployees = await context.Employees.ToListAsync();
        employees = employees.Where(e => !currentEmployees.Any(ce => ce.EmployeeCode == e.EmployeeCode)).ToList();
        await context.Employees.AddRangeAsync(employees);
        logger.LogInformation("Created {Count} default employees", employees.Count);
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
                IsActive = true,
                ManagerId = manager.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            },
            new ShiftTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Standard Shift (9:00 - 18:00)",
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(18, 0, 0),
                IsActive = true,
                ManagerId = manager.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            },
            new ShiftTemplate
            {
                Id = Guid.NewGuid(),
                Name = "Late Morning Shift (10:00 - 19:00)",
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(19, 0, 0),
                IsActive = true,
                ManagerId = manager.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            }
        };

        await context.ShiftTemplates.AddRangeAsync(shiftTemplates);
        logger.LogInformation("Created {Count} shift templates", shiftTemplates.Count);
    }

    #endregion

    #region Holidays

    private async Task SeedHolidaysAsync()
    {
        if (await context.Holidays.AnyAsync())
        {
            logger.LogInformation("Holidays already seeded");
            return;
        }

        logger.LogInformation("Seeding Vietnam holidays...");

        var currentYear = DateTime.Now.Year;
        var holidays = VietnamHolidays.GetDefaultHolidays(currentYear);

        // Set audit fields
        foreach (var holiday in holidays)
        {
            holiday.CreatedAt = DateTime.Now;
            holiday.CreatedBy = "System";
        }

        await context.Holidays.AddRangeAsync(holidays);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} Vietnam holidays for year {Year}", holidays.Count, currentYear);
    }

    #endregion

}
