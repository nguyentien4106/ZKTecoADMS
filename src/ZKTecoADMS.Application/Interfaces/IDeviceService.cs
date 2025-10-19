using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

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
    Task<IEnumerable<DeviceCommand>> GetAllDeviceCommandsAsync(Guid deviceId);
    Task MarkCommandAsSentAsync(Guid commandId);
    Task UpdateCommandStatusAsync(long commandId, CommandStatus status, string? responseData, string? errorMessage);
    Task DeleteDeviceAsync(Guid deviceId);
    
    Task<bool> ValidPinDevice(Guid deviceId, string pin);
}