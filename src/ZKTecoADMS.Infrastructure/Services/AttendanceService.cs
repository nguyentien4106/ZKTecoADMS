using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Services;


public class AttendanceService(
    IAttendanceRepository attendanceRepository,
    ZKTecoDbContext context,
    ILogger<AttendanceService> logger)
    : IAttendanceService
{
    public async Task SaveAttendanceLogsAsync(IEnumerable<Attendance> logs)
    {
        foreach (var log in logs)
        {
            // Check for duplicates
            var exists = await attendanceRepository.LogExistsAsync(
                log.DeviceId, log.PIN, log.AttendanceTime);

            if (!exists)
            {
                // Try to find user by PIN
                var user = await context.UserDevices
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

    public async Task<IEnumerable<Attendance>> GetAttendanceByDeviceAsync(
        Guid deviceId, DateTime? startDate, DateTime? endDate)
    {
        return await attendanceRepository.GetByDeviceIdAsync(deviceId, startDate, endDate);
    }

    public async Task<IEnumerable<Attendance>> GetAttendanceByUserAsync(
        Guid userId, DateTime? startDate, DateTime? endDate)
    {
        return await attendanceRepository.GetByUserIdAsync(userId, startDate, endDate);
    }
}