using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

public interface IDeviceService
{
    Task<Device> GetOrCreateDeviceAsync(string serialNumber);
    Task<Device?> GetDeviceBySerialNumberAsync(string serialNumber);
    Task UpdateDeviceHeartbeatAsync(string serialNumber);
    Task<IEnumerable<DeviceCommand>> GetPendingCommandsAsync(Guid deviceId);
    
    Task<IEnumerable<DeviceCommand>> GetCommandsAsync(Guid deviceId);
    
    Task<DeviceCommand> CreateCommandAsync(DeviceCommand command);
    Task MarkCommandAsSentAsync(Guid commandId);
    Task UpdateCommandStatusAsync(long commandId, CommandStatus status, string? responseData, string? errorMessage);
    Task<AppResponse<bool>> IsValidUserAsync(User user);
}