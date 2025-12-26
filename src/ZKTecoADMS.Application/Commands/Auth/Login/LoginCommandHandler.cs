using ZKTecoADMS.Application.DTOs.Auth;
using ZKTecoADMS.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ZKTecoADMS.Application.Commands.Auth.Login;

/// <summary>
/// Handles user login without throwing exceptions.
/// Returns appropriate AppResponse with success/error status and messages.
/// Validates email, password, account status, and generates JWT tokens on successful login.
/// </summary>
public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    IAuthenticateService authenticateService
    ) : ICommandHandler<LoginCommand, AppResponse<AuthenticateResponse>>
{
    public async Task<AppResponse<AuthenticateResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // First, find the user by username (lightweight query for validation)
        var user = await userManager.Users
            .Where(e => e.UserName == request.UserName)
            .Include(e => e.Employee)
            .Include(e => e.Manager)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user == null)
        {
            return AppResponse<AuthenticateResponse>.Error($"{request.UserName} is not found.");
        }
        
        // Check if the user's email is confirmed (if required)
        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            return AppResponse<AuthenticateResponse>.Error("Email not confirmed. Please check your email and confirm your account.");
        }

        // Check if the user account is locked out
        if (await userManager.IsLockedOutAsync(user))
        {   
            return AppResponse<AuthenticateResponse>.Error("Account is locked out. Please try again later.");
        }

        // Validate password
        var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            // Record failed login attempt
            await userManager.AccessFailedAsync(user);
            return AppResponse<AuthenticateResponse>.Error("Password is incorrect.");
        }

        // Reset failed login attempts on successful login
        await userManager.ResetAccessFailedCountAsync(user);

        // Now reload the user with all navigation properties for token generation
        var userWithIncludes = await userManager.Users
            .Include(u => u.Employee)
            .Include(u => u.Manager)
            .SingleOrDefaultAsync(u => u.Id == user.Id, cancellationToken);

        // Generate tokens with fully loaded user entity
        return await authenticateService.Authenticate(userWithIncludes!, cancellationToken);
    }
}