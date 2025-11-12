using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Devices.CreateDeviceCmd;

public class CreateDeviceCmdHandler(IRepository<Device> deviceRepository, IRepository<DeviceCommand> deviceCmdRepository) : ICommandHandler<CreateDeviceCmdCommand, AppResponse<DeviceCmdDto>>
{
    public async Task<AppResponse<DeviceCmdDto>> Handle(CreateDeviceCmdCommand request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.DeviceId, cancellationToken: cancellationToken);

        var command = new DeviceCommand
        {
            DeviceId = device.Id,
            Command = GetCommand((DeviceCommandTypes)request.CommandType),
            Priority = request.Priority
        };
        
        var created = await deviceCmdRepository.AddAsync(command, cancellationToken);

        return AppResponse<DeviceCmdDto>.Success(created.Adapt<DeviceCmdDto>());
    }

    private static string GetCommand(DeviceCommandTypes commandType)
    {
        return commandType switch
        {
            DeviceCommandTypes.ClearAttendances => "CLEAR LOG",
            DeviceCommandTypes.ClearUsers => "CLEAR ALL USERINFO",
            DeviceCommandTypes.ClearData => "CLEAR DATA",
            DeviceCommandTypes.RestartDevice => "REBOOT",
            DeviceCommandTypes.SyncAttendances => ClockCommandBuilder.BuildGetAttendanceCommand(DateTime.UtcNow.AddYears(-5), DateTime.UtcNow),
            _ => ""
        };
    }
}

