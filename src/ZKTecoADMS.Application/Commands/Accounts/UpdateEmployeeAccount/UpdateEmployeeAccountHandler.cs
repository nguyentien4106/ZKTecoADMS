using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Accounts.UpdateEmployeeAccount;

public class UpdateEmployeeAccountHandler(
    UserManager<ApplicationUser> userManager,
    IRepository<User> userRepository
    ) : ICommandHandler<UpdateEmployeeAccountCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(UpdateEmployeeAccountCommand request, CancellationToken cancellationToken)
    {
        if (request.UserDeviceId == Guid.Empty)
        {
            return AppResponse<bool>.Error("UserDeviceId must be provided.");
        }

        // Get the user device
        var userDevice = await userRepository.GetByIdAsync(request.UserDeviceId);
        if (userDevice == null)
        {
            return AppResponse<bool>.Error("User device not found.");
        }

        // Check if the user has an associated application user
        if (userDevice.ApplicationUserId == null)
        {
            return AppResponse<bool>.Error("User does not have an associated account.");
        }

        // Get the application user
        var applicationUser = await userManager.FindByIdAsync(userDevice.ApplicationUserId.ToString()!);
        if (applicationUser == null)
        {
            return AppResponse<bool>.Error("Associated account not found.");
        }

        // Update application user properties
        applicationUser.FirstName = request.FirstName;
        applicationUser.LastName = request.LastName;
        applicationUser.PhoneNumber = request.PhoneNumber;

        var updateResult = await userManager.UpdateAsync(applicationUser);
        if (!updateResult.Succeeded)
        {
            return AppResponse<bool>.Error(updateResult.Errors.Select(e => e.Description).ToList());
        }

        // Update password if provided
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(applicationUser);
            var passwordResult = await userManager.ResetPasswordAsync(applicationUser, token, request.Password);
            if (!passwordResult.Succeeded)
            {
                return AppResponse<bool>.Error(passwordResult.Errors.Select(e => e.Description).ToList());
            }
        }

        return AppResponse<bool>.Success(true);
    }
}
