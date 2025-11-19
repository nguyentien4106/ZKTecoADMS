using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;

/// <summary>
/// Strategy for handling DeleteUser command responses
/// </summary>
[DeviceCommandStrategy(DeviceCommandTypes.DeleteEmployee)]
public class DeleteUserStrategy(
    IRepository<Employee> employeeRepository,
    UserManager<ApplicationUser> userManager) : IDeviceCommandStrategy
{
    public async Task ExecuteAsync(Device device, Guid objectRefId, ClockCommandResponse response, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(objectRefId, cancellationToken: cancellationToken);
        if (employee != null && response.IsSuccess)
        {
            await employeeRepository.DeleteAsync(employee, cancellationToken);
            if(employee.ApplicationUserId == null)
            {
                return;
            }
            
            var employeeAccount = await userManager.FindByIdAsync(employee.ApplicationUserId.ToString());
            if (employeeAccount != null)
            {
                await userManager.DeleteAsync(employeeAccount);
            }
        }
    }
}
