namespace ZKTecoADMS.Application.DTOs.Devices;
public class DeviceDto
{
    public Guid Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string? Model { get; set; }
    public string? IpAddress { get; set; }
    public string DeviceStatus { get; set; } = string.Empty;
    public DateTime? LastOnline { get; set; }
    public bool IsActive { get; set; }
}