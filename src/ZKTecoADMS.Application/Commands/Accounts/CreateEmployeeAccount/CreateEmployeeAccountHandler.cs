using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Accounts.CreateEmployeeAccount;

public class CreateEmployeeAccountHandler(
    UserManager<ApplicationUser> userManager,
    IRepository<User> userRepository
    ) : ICommandHandler<CreateEmployeeAccountCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(CreateEmployeeAccountCommand request, CancellationToken cancellationToken)
    {
        if(request.UserDeviceId == Guid.Empty){
            return AppResponse<bool>.Error("UserDeviceId must be provided to link the employee account.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Created = DateTime.Now,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var roleResult = await userManager.AddToRoleAsync(user, nameof(Roles.Employee));
            if (roleResult.Succeeded)
            {
                var userDevice = await userRepository.GetByIdAsync(request.UserDeviceId);
                if (userDevice != null)
                {
                    userDevice.ApplicationUserId = user.Id;
                    await userRepository.UpdateAsync(userDevice);
                    return AppResponse<bool>.Success(true);
                }
                else
                {
                    return AppResponse<bool>.Error("User device not found.");
                }

            }

            return AppResponse<bool>.Error(roleResult.Errors.Select(e => e.Description).ToList());
        }

        return AppResponse<bool>.Error(result.Errors.Select(e => e.Description).ToList());
    }
}