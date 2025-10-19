using Microsoft.EntityFrameworkCore;
using ZKTeco.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Infrastructure.Data;

namespace ZKTecoADMS.Infrastructure.Repositories;


public class DeviceRepository : Repository<Device>, IDeviceRepository
{
    public DeviceRepository(ZKTecoDbContext context) : base(context)
    {
    }

    public async Task<Device?> GetBySerialNumberAsync(string serialNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(d => d.SerialNumber == serialNumber);
    }

    public async Task<IEnumerable<Device>> GetActiveDevicesAsync()
    {
        return await _dbSet
            .Where(d => d.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetDevicesWithPendingCommandsAsync()
    {
        return await _dbSet
            .Include(d => d.DeviceCommands)
            .Where(d => d.DeviceCommands.Any(c => c.Status == CommandStatus.Created))
            .ToListAsync();
    }

    public async Task UpdateLastOnlineAsync(Guid deviceId, DateTime timestamp)
    {
        var device = await GetByIdAsync(deviceId);
        if (device != null)
        {
            device.LastOnline = timestamp;
            device.DeviceStatus = "Online";
            device.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(device);
        }
    }
}