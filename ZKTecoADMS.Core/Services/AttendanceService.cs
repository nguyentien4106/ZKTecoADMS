using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Infrastructure.Data;
using ZKTecoADMS.Infrastructure.Repositories;

namespace ZKTecoADMS.Core.Services;


public class AttendanceService(
    IAttendanceRepository attendanceRepository,
    ZKTecoDbContext context,
    ILogger<AttendanceService> logger)
    : IAttendanceService
{
    public async Task SaveAttendanceLogsAsync(IEnumerable<AttendanceLog> logs)
    {
        foreach (var log in logs)
        {
            // Check for duplicates
            var exists = await attendanceRepository.LogExistsAsync(
                log.DeviceId, log.PIN, log.AttendanceTime);

            if (!exists)
            {
                // Try to find user by PIN
                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.PIN == log.PIN);
                
                if (user != null)
                {
                    log.UserId = user.Id;
                }

                await attendanceRepository.AddAsync(log);
                logger.LogInformation("Saved attendance log: Device={DeviceId}, PIN={PIN}, Time={Time}", 
                    log.DeviceId, log.PIN, log.AttendanceTime);
            }
            else
            {
                logger.LogDebug("Duplicate attendance log skipped: Device={DeviceId}, PIN={PIN}, Time={Time}", 
                    log.DeviceId, log.PIN, log.AttendanceTime);
            }
        }
    }

    public async Task<IEnumerable<AttendanceLog>> GetAttendanceByDeviceAsync(
        Guid deviceId, DateTime? startDate, DateTime? endDate)
    {
        return await attendanceRepository.GetByDeviceIdAsync(deviceId, startDate, endDate);
    }

    public async Task<IEnumerable<AttendanceLog>> GetAttendanceByUserAsync(
        Guid userId, DateTime? startDate, DateTime? endDate)
    {
        return await attendanceRepository.GetByUserIdAsync(userId, startDate, endDate);
    }

    public async Task<IEnumerable<AttendanceLog>> GetUnprocessedLogsAsync()
    {
        return await attendanceRepository.GetUnprocessedLogsAsync();
    }

    public async Task MarkLogsAsProcessedAsync(IEnumerable<Guid> logIds)
    {
        var logs = await context.AttendanceLogs
            .Where(l => logIds.Contains(l.Id))
            .ToListAsync();

        foreach (var log in logs)
        {
            log.IsProcessed = true;
            log.ProcessedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();
    }
}