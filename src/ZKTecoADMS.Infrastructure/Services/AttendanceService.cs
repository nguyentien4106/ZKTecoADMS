using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Services;


public class AttendanceService(
    IRepository<Attendance> attendanceRepository,
    IShiftService shiftService
)
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

    public async Task<IEnumerable<Attendance>> GetAttendanceByEmployeeAsync(
        Guid deviceId, Guid employeeId, DateTime? startDate, DateTime? endDate)
    {
        return await attendanceRepository.GetAllAsync(
            a => a.DeviceId == deviceId && a.AttendanceTime.Date >= startDate && a.AttendanceTime.Date <= endDate && a.EmployeeId == employeeId,
            orderBy: query => query.OrderByDescending(a => a.AttendanceTime.Date)
        );    }

    public async Task<bool> LogExistsAsync(Guid deviceId, string pin, DateTime attendanceTime)
    {
        return await attendanceRepository.ExistsAsync(a => 
            a.DeviceId == deviceId && 
            a.PIN == pin && 
            a.AttendanceTime == attendanceTime);
    }

    public async Task CreateAttendancesAsync(IEnumerable<Attendance> attendances)
    {
        await attendanceRepository.AddRangeAsync(attendances);

        // Reload attendances with Employee navigation property for shift mapping
        var attendanceIds = attendances.Select(a => a.Id).ToList();
        var attendancesWithEmployee = await attendanceRepository.GetAllAsync(
            filter: a => attendanceIds.Contains(a.Id),
            includeProperties: new[] { nameof(Attendance.Employee) }
        );

        await UpdateShiftAttendancesAsync(attendancesWithEmployee);
    }

    public async Task<bool> UpdateShiftAttendancesAsync(IEnumerable<Attendance> attendances)
    {
        await shiftService.UpdateShiftAttendancesAsync(attendances);
        return true;
    }
}