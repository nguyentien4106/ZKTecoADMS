using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Repositories;

public class ShiftRepository(
    ZKTecoDbContext context,
    ILogger<ShiftRepository> logger) : EfRepository<Shift>(context, logger), IShiftRepository
{
    public async Task<List<Shift>> GetShiftsByApplicationUserIdAsync(Guid applicationUserId, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Where(s => s.ApplicationUserId == applicationUserId)
            .Include(s => s.ApplicationUser)
            .Include(s => s.ApprovedByUser)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Shift>> GetPendingShiftsAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Where(s => s.Status == ShiftStatus.Pending)
            .Include(s => s.ApplicationUser)
            .OrderBy(s => s.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Shift>> GetShiftsByStatusAsync(ShiftStatus status, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Where(s => s.Status == status)
            .Include(s => s.ApplicationUser)
            .Include(s => s.ApprovedByUser)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Shift>> GetShiftsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Where(s => s.StartTime >= startDate && s.EndTime <= endDate)
            .Include(s => s.ApplicationUser)
            .Include(s => s.ApprovedByUser)
            .OrderBy(s => s.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Shift>> GetShiftsByManagerAsync(Guid managerId, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(s => s.ApplicationUser)
            .Include(s => s.ApprovedByUser)
            .Where(s => s.ApplicationUser != null && s.ApplicationUser.ManagerId == managerId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
