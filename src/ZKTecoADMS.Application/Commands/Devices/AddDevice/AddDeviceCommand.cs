using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Commands.Devices.AddDevice;

public record AddDeviceCommand(
    string SerialNumber,
    string DeviceName,
    string? Location,
    string? Description,
    Guid ApplicationUserId) : ICommand<AppResponse<DeviceDto>>;