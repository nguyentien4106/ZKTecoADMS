using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Commands.DeviceCommands.CreateDeviceCmd;

public record CreateDeviceCmdCommand(Guid DeviceId, int CommandType, int Priority) : ICommand<AppResponse<DeviceCmdDto>>;
