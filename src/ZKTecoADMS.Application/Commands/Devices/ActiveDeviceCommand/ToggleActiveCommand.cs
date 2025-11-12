using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Commands.Devices.ActiveDeviceCommand;

public record ToggleActiveCommand(Guid DeviceId) : ICommand<AppResponse<DeviceDto>>;