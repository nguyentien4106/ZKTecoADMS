using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.DeviceCommands.CreateDeviceCmd;

public class CreateDeviceCmdHandler(IRepository<Device> deviceRepository, IRepository<DeviceCommand> deviceCmdRepository) : ICommandHandler<CreateDeviceCmdCommand, AppResponse<DeviceCmdDto>>
{
    public async Task<AppResponse<DeviceCmdDto>> Handle(CreateDeviceCmdCommand request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.DeviceId, cancellationToken: cancellationToken);
        if (device == null)
        {
            return AppResponse<DeviceCmdDto>.Fail("Device not found");
        }
        var commandType = (DeviceCommandTypes)request.CommandType;
        var commandStr =  GetCommand(commandType, request.DeviceId);
        
        var command = new DeviceCommand
        {
            DeviceId = device.Id,
            Command = commandStr,
            Priority = request.Priority,
            CommandType = commandType
        };
        
        var created = await deviceCmdRepository.AddAsync(command, cancellationToken);

        return AppResponse<DeviceCmdDto>.Success(created.Adapt<DeviceCmdDto>());
    }

    private static string GetCommand(DeviceCommandTypes commandType, Guid id)
    {
        return commandType switch
        {
            DeviceCommandTypes.ClearAttendances => "CLEAR LOG",
            DeviceCommandTypes.ClearDeviceUsers => "CLEAR ALL USERINFO",
            DeviceCommandTypes.ClearData => "CLEAR DATA",
            DeviceCommandTypes.RestartDevice => "REBOOT",
            DeviceCommandTypes.SyncAttendances => ClockCommandBuilder.BuildGetAttendanceCommand(DateTime.Now.AddYears(-5), DateTime.Now),
            DeviceCommandTypes.SyncDeviceUsers => ClockCommandBuilder.BuildGetAllUsersCommand(),
            _ => "NOT IMPLEMENTED"
        };
    }
}
