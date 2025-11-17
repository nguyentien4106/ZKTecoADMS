using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

public interface IAttendanceService 
{
    Task<IEnumerable<Attendance>> GetAttendanceByDeviceAsync(Guid deviceId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<Attendance>> GetAttendanceByEmployeeAsync(Guid deviceId, Guid employeeId, DateTime? startDate, DateTime? endDate);

    Task<bool> LogExistsAsync(Guid deviceId, string pin, DateTime attendanceTime);
}
