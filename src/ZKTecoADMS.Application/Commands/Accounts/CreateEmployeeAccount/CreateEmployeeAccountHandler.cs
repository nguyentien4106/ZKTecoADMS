using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Accounts.CreateEmployeeAccount;

public class CreateEmployeeAccountHandler(
    UserManager<ApplicationUser> userManager,
    IRepository<Employee> employeeRepository
    ) : ICommandHandler<CreateEmployeeAccountCommand, AppResponse<AccountDto>>
{
    public async Task<AppResponse<AccountDto>> Handle(CreateEmployeeAccountCommand request, CancellationToken cancellationToken)
    {
        if(request.EmployeeDeviceId == Guid.Empty){
            return AppResponse<AccountDto>.Error("EmployeeDeviceId must be provided to link the employee account.");
        }

        // Validate manager exists if ManagerId is provided
        var manager = await userManager.FindByIdAsync(request.ManagerId.ToString());
        if (manager == null)
        {
            return AppResponse<AccountDto>.Error("Manager not found.");
        }

        // Optionally: Verify the manager has the Manager role
        var isManager = await userManager.IsInRoleAsync(manager, nameof(Roles.Manager));
        if (!isManager)
        {
            return AppResponse<AccountDto>.Error("The specified user is not a manager.");
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
            PhoneNumberConfirmed = true,
            ManagerId  = request.ManagerId,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return AppResponse<AccountDto>.Error(result.Errors.Select(e => e.Description).ToList());
        }
        var roleResult = await userManager.AddToRoleAsync(user, nameof(Roles.Employee));
        if (!roleResult.Succeeded)
        {
            return AppResponse<AccountDto>.Error(roleResult.Errors.Select(e => e.Description).ToList());
        }
        var employeeDevice = await employeeRepository.GetByIdAsync(request.EmployeeDeviceId, cancellationToken: cancellationToken);

        if (employeeDevice == null)
        {
            return AppResponse<AccountDto>.Error("Employee device not found.");
        }
        
        employeeDevice.ApplicationUserId = user.Id;
        await employeeRepository.UpdateAsync(employeeDevice, cancellationToken);

        return AppResponse<AccountDto>.Success(new AccountDto
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
        });
    }
}