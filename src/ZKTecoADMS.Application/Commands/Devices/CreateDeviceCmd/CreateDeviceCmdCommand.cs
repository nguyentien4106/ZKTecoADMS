using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Commands.Devices.CreateDeviceCmd;

public record CreateDeviceCmdCommand(Guid DeviceId, int CommandType, int Priority) : ICommand<AppResponse<DeviceCmdDto>>;