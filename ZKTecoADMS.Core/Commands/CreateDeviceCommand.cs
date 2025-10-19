using MediatR;
using ZKTeco.Domain.Entities;

namespace ZKTecoADMS.Core.Commands;

public class CreateDeviceCommand : IRequest<Device>
{
    public string SerialNumber { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string? Model { get; set; }
    public string? IpAddress { get; set; }
    public int? Port { get; set; }
    public string? Location { get; set; }
}