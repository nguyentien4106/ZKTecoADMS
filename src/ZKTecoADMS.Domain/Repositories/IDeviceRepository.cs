using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Repositories;

public interface IDeviceRepository : IRepository<Device>
{
    Task<Device?> GetBySerialNumberAsync(string serialNumber);
    Task UpdateLastOnlineAsync(Guid deviceId, DateTime timestamp);
}
