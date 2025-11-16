using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Services;


public class AttendanceService(
    IRepository<Attendance> attendanceRepository)
    : IAttendanceService
{
    public async Task<IEnumerable<Attendance>> GetAttendanceByDeviceAsync(
        Guid deviceId, DateTime? startDate, DateTime? endDate)
    {
        return await attendanceRepository.GetAllAsync(
            a => a.DeviceId == deviceId && a.AttendanceTime.Date >= startDate && a.AttendanceTime.Date <= endDate,
            orderBy: query => query.OrderByDescending(a => a.AttendanceTime.Date)
        );
    }

    public async Task<IEnumerable<Attendance>> GetAttendanceByUserAsync(
        Guid deviceId, Guid userId, DateTime? startDate, DateTime? endDate)
    {
        return await attendanceRepository.GetAllAsync(
            a => a.DeviceId == deviceId && a.AttendanceTime.Date >= startDate && a.AttendanceTime.Date <= endDate && a.UserId == userId,
            orderBy: query => query.OrderByDescending(a => a.AttendanceTime.Date)
        );    }

    public async Task<bool> LogExistsAsync(Guid deviceId, string pin, DateTime attendanceTime)
    {
        return await attendanceRepository.ExistsAsync(a => 
            a.DeviceId == deviceId && 
            a.PIN == pin && 
            a.AttendanceTime == attendanceTime);
    }
}