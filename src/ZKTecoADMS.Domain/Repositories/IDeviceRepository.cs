using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<Device?> GetBySerialNumberAsync(string serialNumber);
    Task<IEnumerable<Device>> GetActiveDevicesAsync();
    Task<IEnumerable<Device>> GetDevicesWithPendingCommandsAsync();
    Task UpdateLastOnlineAsync(Guid deviceId, DateTime timestamp);
}
