using ZKTecoADMS.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ZKTecoADMS.Api.Controllers.Base;

[Authorize]
public abstract class AuthenticatedControllerBase : ControllerBase
{
    protected Guid CurrentUserId => GetCurrentUserId();
    
    protected string CurrentUserRole => GetCurrentUserRole();
    
    protected bool IsAdmin => CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    
    protected bool IsManager => CurrentUserRole.Equals("Manager", StringComparison.OrdinalIgnoreCase);
    
    protected bool IsEmployee => CurrentUserRole.Equals("Employee", StringComparison.OrdinalIgnoreCase);

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
} 