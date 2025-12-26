using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Accounts.UpdateEmployeeAccount;

public class UpdateEmployeeAccountHandler(
    UserManager<ApplicationUser> userManager,
    IRepository<DeviceUser> employeeRepository
    ) : ICommandHandler<UpdateEmployeeAccountCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(UpdateEmployeeAccountCommand request, CancellationToken cancellationToken)
    {
        if (request.EmployeeDeviceId == Guid.Empty)
        {
            return AppResponse<bool>.Error("EmployeeDeviceId must be provided.");
        }

        // Get the employee device
        var employeeDevice = await employeeRepository.GetByIdAsync(request.EmployeeDeviceId);
        if (employeeDevice == null)
        {
            return AppResponse<bool>.Error("Employee device not found.");
        }

        
        return AppResponse<bool>.Success(true);
    }
}
