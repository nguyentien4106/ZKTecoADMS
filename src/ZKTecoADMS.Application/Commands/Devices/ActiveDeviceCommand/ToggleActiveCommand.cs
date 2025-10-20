namespace ZKTecoADMS.Application.Commands.Devices.ActiveDeviceCommand;

public record ToggleActiveCommand(Guid DeviceId) : ICommand<AppResponse<bool>>;