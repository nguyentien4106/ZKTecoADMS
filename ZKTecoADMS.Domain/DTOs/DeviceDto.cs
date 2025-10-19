using System;
using System.Collections.Generic;

namespace ZKTecoADMS.Domain.DTOs;

public class DeviceDto
{
    public int Id { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string? Model { get; set; }
    public string? IpAddress { get; set; }
    public int? Port { get; set; }
    public string? Location { get; set; }
    public string Timezone { get; set; } = "UTC";
    public bool IsActive { get; set; }
    public DateTime? LastOnline { get; set; }
    public string? FirmwareVersion { get; set; }
    public string? Platform { get; set; }
    public string DeviceStatus { get; set; } = "Offline";
    public int? MaxUsers { get; set; }
    public int? MaxFingerprints { get; set; }
    public int? MaxFaces { get; set; }
    public bool SupportsPushSDK { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}