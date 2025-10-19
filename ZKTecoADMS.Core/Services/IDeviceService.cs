using ZKTeco.Domain.Entities;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Core.Services;

public interface IDeviceService
{
    Task<Device> GetOrCreateDeviceAsync(string serialNumber);
    Task<Device?> GetDeviceBySerialNumberAsync(string serialNumber);
    Task<Device?> GetDeviceByIdAsync(Guid id);
    Task<IEnumerable<Device>> GetAllDevicesAsync();
    Task<Device> RegisterDeviceAsync(Device device);
    Task UpdateDeviceHeartbeatAsync(string serialNumber);
    Task<IEnumerable<DeviceCommand>> GetPendingCommandsAsync(Guid deviceId);
    Task<DeviceCommand> CreateCommandAsync(DeviceCommand command);
    Task MarkCommandAsSentAsync(Guid commandId);
    Task UpdateCommandStatusAsync(Guid commandId, CommandStatus status, string? responseData);
    Task DeleteDeviceAsync(Guid deviceId);
}