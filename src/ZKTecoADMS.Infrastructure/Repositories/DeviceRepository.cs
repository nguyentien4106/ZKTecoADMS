using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Infrastructure.Repositories;


public class DeviceRepository(ZKTecoDbContext context, ILogger<EfRepository<Device>> logger) : EfRepository<Device>(context, logger), IDeviceRepository
{

    public async Task<Device?> GetBySerialNumberAsync(string serialNumber)
    {
        return await dbSet
            .FirstOrDefaultAsync(d => d.SerialNumber == serialNumber);
    }

    public async Task<IEnumerable<Device>> GetActiveDevicesAsync()
    {
        return await dbSet
            .Where(d => d.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetDevicesWithPendingCommandsAsync()
    {
        return await dbSet
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