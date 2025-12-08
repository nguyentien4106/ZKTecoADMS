using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Services;


public class AttendanceService(
    IRepository<Attendance> attendanceRepository,
    IShiftService shiftService,
    IRepository<Employee> employeeRepository,
    IRepository<Shift> shiftRepository,
    ILogger<AttendanceService> logger
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

    public async Task<bool> UpdateShiftAttendancesAsync(IEnumerable<Attendance> attendances, Device device)
    {
        foreach (var attendance in attendances.OrderBy(a => a.AttendanceTime))
        {
            if(attendance.EmployeeId == null)
            {
                logger.LogWarning("{DeviceSN}:Attendance with ID {AttendanceId} has no associated EmployeeId.", device.SerialNumber, attendance.Id);
                continue;
            }   

            var employeeUser = await employeeRepository.GetSingleAsync(
                filter: e => e.DeviceId == device.Id && e.Pin == attendance.PIN
            );

            if(employeeUser == null)
            {
                logger.LogWarning("{DeviceSN}:No employee found for Attendance ID {AttendanceId} with PIN {PIN}.", device.SerialNumber, attendance.Id, attendance.PIN);
                continue;
            }
            
            
            var shift = await shiftService.GetShiftByDateAsync(employeeUser.ApplicationUserId, attendance.AttendanceTime.Date);
            if (shift == null)
            {
                logger.LogWarning("{DeviceSN}:No shift found for Employee ID {EmployeeId} on {AttendanceDate}.", device.SerialNumber, employeeUser.ApplicationUserId, attendance.AttendanceTime.Date);
                continue;
            }

            if(shift.CheckInAttendanceId == null)
            {
                logger.LogInformation("{DeviceSN}:Linking Attendance ID {AttendanceId} as Check-In for Shift ID {ShiftId}.", device.SerialNumber, attendance.Id, shift.Id);
                shift.CheckInAttendanceId = attendance.Id;
            }
            else
            {
                logger.LogInformation("{DeviceSN}:Linking Attendance ID {AttendanceId} as Check-Out for Shift ID {ShiftId}.", device.SerialNumber, attendance.Id, shift.Id);
                shift.CheckOutAttendanceId = attendance.Id;
            }

            await shiftRepository.UpdateAsync(shift);
        }
        return true;
    }
}