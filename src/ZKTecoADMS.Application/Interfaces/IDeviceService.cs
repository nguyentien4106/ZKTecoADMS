using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

public interface IDeviceService
{
    Task<Device?> GetDeviceBySerialNumberAsync(string serialNumber);
    Task<bool> IsExistDeviceAsync(string serialNumber);
    Task UpdateDeviceHeartbeatAsync(string serialNumber);
    Task<IEnumerable<DeviceCommand>> GetPendingCommandsAsync(Guid deviceId);
    Task<DeviceCommand> CreateCommandAsync(DeviceCommand command);
    Task<AppResponse<bool>> IsValidUserAsync(User user);
    
    Task<IEnumerable<Device>> GetAllDevicesByUserAsync(Guid userId);
}