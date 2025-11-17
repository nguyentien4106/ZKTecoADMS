using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

public interface IDeviceService
{
    Task<Device?> GetDeviceBySerialNumberAsync(string serialNumber);
    Task<bool> IsExistDeviceAsync(string serialNumber);
    Task UpdateDeviceHeartbeatAsync(string serialNumber);
    Task<IEnumerable<DeviceCommand>> GetPendingCommandsAsync(Guid deviceId);
    Task<DeviceCommand> CreateCommandAsync(DeviceCommand command);
    Task<AppResponse<bool>> IsValidEmployeeAsync(Employee employee);
    
    Task<IEnumerable<Device>> GetAllDevicesByEmployeeAsync(Guid employeeId);
    
    Task<IEnumerable<Device>> GetAllDevicesAsync();
}