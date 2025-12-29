using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.DeviceUsers;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.DeviceUsers.Create;

public class CreateDeviceUserHandler(
    IDeviceService deviceService, 
    IRepository<DeviceUser> employeeRepository,
    IRepository<DeviceCommand> deviceCmdRepository) : ICommandHandler<CreateDeviceUserCommand, AppResponse<DeviceUserDto>>
{
    public async Task<AppResponse<DeviceUserDto>> Handle(CreateDeviceUserCommand request, CancellationToken cancellationToken)
    {
        var deviceUser = request.Adapt<DeviceUser>();
        deviceUser.IsActive = false;
        
        var validEmployee = await deviceService.IsUserValid(deviceUser);
        if (!validEmployee.IsSuccess)
        {
            return AppResponse<DeviceUserDto>.Fail(validEmployee.Message);
        }
        
        var employeeEntity = await employeeRepository.AddAsync(deviceUser, cancellationToken);
        
        var cmd = new DeviceCommand
        {
            DeviceId = employeeEntity.DeviceId,
            Command = ClockCommandBuilder.BuildAddOrUpdateEmployeeCommand(employeeEntity),
            Priority = 10,
            CommandType = DeviceCommandTypes.AddDeviceUser,
            ObjectReferenceId = employeeEntity.Id
        };
        await deviceCmdRepository.AddAsync(cmd, cancellationToken);

        return AppResponse<DeviceUserDto>.Success(employeeEntity.Adapt<DeviceUserDto>());

    }
}