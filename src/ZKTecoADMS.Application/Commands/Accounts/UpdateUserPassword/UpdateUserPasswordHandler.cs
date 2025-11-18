using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Commands.Accounts.UpdateUserPassword;

public class UpdateUserPasswordHandler(UserManager<ApplicationUser> userManager) 
    : ICommandHandler<UpdateUserPasswordCommand, AppResponse<UserProfileDto>>
{
    public async Task<AppResponse<UserProfileDto>> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .Include(u => u.Manager)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            return AppResponse<UserProfileDto>.Error("User not found");
        }

        // Change password
        var passwordResult = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!passwordResult.Succeeded)
        {
            var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
            return AppResponse<UserProfileDto>.Error(errors);
        }

        // Refresh user data
        user = await userManager.Users
            .Include(u => u.Manager)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        var roles = await userManager.GetRolesAsync(user!);

        var profile = new UserProfileDto
        {
            Id = user!.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Roles = roles.ToList(),
            ManagerId = user.ManagerId,
            ManagerName = user.Manager != null ? $"{user.Manager.FirstName} {user.Manager.LastName}" : null,
            Created = user.Created
        };

        return AppResponse<UserProfileDto>.Success(profile);
    }
}
