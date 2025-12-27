using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Application.DTOs.Commons;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Accounts;
public class CreateEmployeeAccountHandler(
    UserManager<ApplicationUser> userManager,
    IRepository<Employee> employeeRepository
) : ICommandHandler<CreateEmployeeAccountCommand, AppResponse<AccountDto>>
{
    public async Task<AppResponse<AccountDto>> Handle(CreateEmployeeAccountCommand request, CancellationToken cancellationToken)
    {
        if (!request.EmployeeId.HasValue)
        {
            return AppResponse<AccountDto>.Error("EmployeeId is required to create an employee account.");
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

        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId.Value);
        
        if(employee == null)
        {
            return AppResponse<AccountDto>.Error("Employee not found.");
        }

        var newUser = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.Now,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            ManagerId  = request.ManagerId
        };

        var result = await userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
        {
            return AppResponse<AccountDto>.Error(result.Errors.Select(e => e.Description).ToList());
        }

        var roleResult = await userManager.AddToRoleAsync(newUser, nameof(Roles.Employee));
        if (!roleResult.Succeeded)
        {
            return AppResponse<AccountDto>.Error(roleResult.Errors.Select(e => e.Description).ToList());
        }

        employee.ApplicationUserId = newUser.Id;
        await employeeRepository.UpdateAsync(employee);

        return AppResponse<AccountDto>.Success(new AccountDto
        {
            Id = newUser.Id,
            Email = request.Email,
            UserName = newUser.UserName!,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            ManagerId = request.ManagerId,
            EmployeeId = request.EmployeeId,
            ManagerName = manager.GetFullName(),
            Roles = [nameof(Roles.Employee)],
            CreatedAt = newUser.CreatedAt
        });
    }
}