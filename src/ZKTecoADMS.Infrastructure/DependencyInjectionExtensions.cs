using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Infrastructure.Interceptors;
using ZKTecoADMS.Infrastructure.Services.Auth;
using ZKTecoADMS.Application.Interfaces.Auth;
using ZKTecoADMS.Application.Settings;
using System.Text;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Repositories;
using ZKTecoADMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Core.Services;
using ZKTecoADMS.Infrastructure.Services;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Core.Services.DeviceOperations;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Infrastructure.Services.DeviceOperations;

namespace ZKTecoADMS.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        ArgumentNullException.ThrowIfNull(jwtSettings, "JwtSettings was missed !");

        services.AddScoped<ZKTecoDbInitializer>();

        // Add services to the container.
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddDbContext<ZKTecoDbContext>((sp, options) =>
        {
            var auditableInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            
            options.UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(ZKTecoDbContext).Assembly.GetName().Name))
                .AddInterceptors(auditableInterceptor);
        });
        services.AddAppIdentity();
        services.AddJwtConfiguration(jwtSettings);
        services.AddApplicationServices();
        services.AddCorsPolicy(jwtSettings);

        return services;
    }

    private static IServiceCollection AddJwtConfiguration(this IServiceCollection services, JwtSettings jwtSettings)
    {

        services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.UseSecurityTokenValidators = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecret)),
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = ClaimTypes.Role
                    };
                });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.AdminOnly,
                    policy => policy.RequireRole(nameof(Roles.Admin)));

            options.AddPolicy(PolicyNames.AtLeastManager,
                    policy => policy.RequireRole(nameof(Roles.Admin), nameof(Roles.Manager)));
            
            options.AddPolicy(PolicyNames.AtLeastEmployee,
                policy => policy.RequireRole(nameof(Roles.Admin), nameof(Roles.Manager), nameof(Roles.Employee)));
        });

        services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build());

        return services;
    }

    private static IServiceCollection AddAppIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    // Configuration for authentication fields
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                })
                .AddEntityFrameworkStores<ZKTecoDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole<Guid>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddEntityFrameworkStores<ZKTecoDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("Default");

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // services.AddScoped<IZKTecoDbContext>(provider => provider.GetRequiredService<ZKTecoDbContext>());
        services.AddScoped<IAuthenticateService, AuthenticateService>();
        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IRefreshTokenValidatorService, RefreshTokenService>();

        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IDeviceCmdService, DeviceCmdService>();
        services.AddScoped<IEmployeeOperationService, EmployeeOperationService>();
        services.AddScoped<IAttendanceOperationService, AttendanceOperationService>();
        services.AddScoped<IShiftService, ShiftService>();
        
        // Repository registration
        services.AddScoped(typeof(IRepositoryPagedQuery<>), typeof(PagedQueryRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(Repository<>), typeof(EfRepository<>));
        
        // Salary profile repositories
        services.AddScoped<ISalaryProfileRepository, SalaryProfileRepository>();
        services.AddScoped<IEmployeeSalaryProfileRepository, EmployeeSalaryProfileRepository>();
        services.AddScoped<IPayslipRepository, PayslipRepository>();
        
        return services;
    }

    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, JwtSettings jwtSettings)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("corsPolicy", builder =>
            {
                builder.WithOrigins([
                            jwtSettings.Audience
                        ])
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
        });

        return services;
    }
}
