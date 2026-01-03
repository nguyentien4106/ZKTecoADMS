using ZKTecoADMS.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Api.Controllers.Base;

[Authorize]
public abstract class AuthenticatedControllerBase : ControllerBase
{
    protected Guid CurrentUserId => GetCurrentUserId();
    
    protected string CurrentUserRole => GetCurrentUserRole();

    protected Guid EmployeeId => GetEmployeeId();
    
    protected Guid ManagerId => GetManagerId();

    protected bool IsAdmin => CurrentUserRole.Equals(nameof(Roles.Admin), StringComparison.OrdinalIgnoreCase);
    
    protected bool IsManager => CurrentUserRole.Equals(nameof(Roles.Manager), StringComparison.OrdinalIgnoreCase);
    
    protected bool IsEmployee => CurrentUserRole.Equals(nameof(Roles.Employee), StringComparison.OrdinalIgnoreCase);
    
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedException("User ID not found in token.");
        }
        return userId;
    }
    
    private string GetCurrentUserRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(roleClaim))
        {
            throw new UnauthorizedException("User role not found in token.");
        }
        return roleClaim;
    }

    private Guid GetEmployeeId()
    {
        var employeeIdClaim = User.FindFirst(ClaimTypeNames.EmployeeId)?.Value;
        if (string.IsNullOrEmpty(employeeIdClaim) || !Guid.TryParse(employeeIdClaim, out var employeeId))
        {
            return Guid.Empty;
        }
        return employeeId;
    }

    private Guid GetManagerId()
    {
        var managerIdClaim = User.FindFirst(ClaimTypeNames.ManagerId)?.Value;
        if (string.IsNullOrEmpty(managerIdClaim) || !Guid.TryParse(managerIdClaim, out var managerId))
        {
            return Guid.Empty;
        }
        return managerId;
    }
} 