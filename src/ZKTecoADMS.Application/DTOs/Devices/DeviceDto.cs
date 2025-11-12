namespace ZKTecoADMS.Application.DTOs.Devices;
public class DeviceDto
{
    public Guid Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime? LastOnline { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; } = string.Empty;
}