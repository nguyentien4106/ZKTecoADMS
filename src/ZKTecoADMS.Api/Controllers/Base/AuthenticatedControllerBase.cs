using ZKTecoADMS.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ZKTecoADMS.Api.Controllers.Base;

[Authorize]
public abstract class AuthenticatedControllerBase : ControllerBase
{
    protected Guid CurrentUserId => GetCurrentUserId();

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedException("User ID not found in token.");
        }
        return userId;
    }
} 