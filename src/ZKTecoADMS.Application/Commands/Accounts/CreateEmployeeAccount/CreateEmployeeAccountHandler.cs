using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Application.DTOs.Commons;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Accounts;
public class CreateEmployeeAccountHandler(
    UserManager<ApplicationUser> userManager
) : ICommandHandler<CreateEmployeeAccountCommand, AppResponse<AccountDto>>
{
    public async Task<AppResponse<AccountDto>> Handle(CreateEmployeeAccountCommand request, CancellationToken cancellationToken)
    {
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
        
        var newUser = new ApplicationUser
        {
            UserName = request.Email.Split("@")[0],
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTime.Now,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            ManagerId  = request.ManagerId,
            EmployeeId = request.EmployeeId
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