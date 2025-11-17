using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Accounts.CreateEmployeeAccount;

public class CreateEmployeeAccountHandler(
    UserManager<ApplicationUser> userManager,
    IRepository<Employee> employeeRepository
    ) : ICommandHandler<CreateEmployeeAccountCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(CreateEmployeeAccountCommand request, CancellationToken cancellationToken)
    {
        if(request.EmployeeDeviceId == Guid.Empty){
            return AppResponse<bool>.Error("EmployeeDeviceId must be provided to link the employee account.");
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
                var employeeDevice = await employeeRepository.GetByIdAsync(request.EmployeeDeviceId);
                if (employeeDevice != null)
                {
                    employeeDevice.ApplicationUserId = user.Id;
                    await employeeRepository.UpdateAsync(employeeDevice);
                    return AppResponse<bool>.Success(true);
                }
                else
                {
                    return AppResponse<bool>.Error("Employee device not found.");
                }

            }

            return AppResponse<bool>.Error(roleResult.Errors.Select(e => e.Description).ToList());
        }

        return AppResponse<bool>.Error(result.Errors.Select(e => e.Description).ToList());
    }
}