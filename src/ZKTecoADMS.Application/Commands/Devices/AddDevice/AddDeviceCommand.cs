using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Commands.Devices.AddDevice;

public record AddDeviceCommand(
    string SerialNumber,
    string DeviceName,
    string? Model,
    string? IpAddress,
    int? Port,
    string? Location,
    Guid ApplicationUserId) : ICommand<AppResponse<DeviceDto>>;