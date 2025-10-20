namespace ZKTecoADMS.Application.DTOs.Devices;

public record AddDeviceRequest(
    string SerialNumber,
    string DeviceName,
    string? Model,
    string? IpAddress,
    int? Port,
    string? Location,
    Guid ApplicationUserId);