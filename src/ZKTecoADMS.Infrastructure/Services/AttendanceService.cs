using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Services;


public class AttendanceService(
    IRepository<Attendance> attendanceRepository,
    IShiftService shiftService,
    IRepository<Employee> employeeRepository
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
    }

    public async Task<bool> UpdateShiftAttendancesAsync(IEnumerable<Attendance> attendances)
    {
        foreach (var attendance in attendances)
        {
            if(attendance.EmployeeId == null)
            {
                continue;
            }
            var employeeUser = await employeeRepository.GetByIdAsync(
                attendance.EmployeeId.Value,
                includeProperties: [nameof(Employee.ApplicationUser)]
            );
            
            var todayShift = await shiftService.GetShiftByDateAsync(employeeUser.Id, attendance.AttendanceTime.Date);
            if (todayShift == null)
            {
                continue;
            }
            if(todayShift.CheckInAttendanceId == null)
            {
                todayShift.CheckInAttendanceId = attendance.Id;
            }
            else
            {
                todayShift.CheckOutAttendanceId = attendance.Id;
            }
            await attendanceRepository.UpdateAsync(attendance);
        }
        return true;
    }
}