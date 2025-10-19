using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Infrastructure.Data;

namespace ZKTecoADMS.Infrastructure.Repositories;


public class AttendanceRepository : Repository<AttendanceLog>, IAttendanceRepository
{
    public AttendanceRepository(ZKTecoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AttendanceLog>> GetByDeviceIdAsync(Guid deviceId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet
            .Include(a => a.User)
            .Where(a => a.DeviceId == deviceId);

        if (startDate.HasValue)
            query = query.Where(a => a.AttendanceTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(a => a.AttendanceTime <= endDate.Value);

        return await query.OrderByDescending(a => a.AttendanceTime).ToListAsync();
    }

    public async Task<IEnumerable<AttendanceLog>> GetByUserIdAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet
            .Include(a => a.Device)
            .Where(a => a.UserId == userId);

        if (startDate.HasValue)
            query = query.Where(a => a.AttendanceTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(a => a.AttendanceTime <= endDate.Value);

        return await query.OrderByDescending(a => a.AttendanceTime).ToListAsync();
    }

    public async Task<IEnumerable<AttendanceLog>> GetUnprocessedLogsAsync()
    {
        return await _dbSet
            .Include(a => a.Device)
            .Include(a => a.User)
            .Where(a => !a.IsProcessed)
            .OrderBy(a => a.AttendanceTime)
            .ToListAsync();
    }

    public async Task<bool> LogExistsAsync(Guid deviceId, string pin, DateTime attendanceTime)
    {
        return await _dbSet.AnyAsync(a => 
            a.DeviceId == deviceId && 
            a.PIN == pin && 
            a.AttendanceTime == attendanceTime);
    }
}