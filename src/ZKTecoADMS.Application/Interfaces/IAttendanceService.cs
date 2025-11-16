using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

public interface IAttendanceService 
{
    Task<IEnumerable<Attendance>> GetAttendanceByDeviceAsync(Guid deviceId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<Attendance>> GetAttendanceByUserAsync(Guid deviceId, Guid userId, DateTime? startDate, DateTime? endDate);

    Task<bool> LogExistsAsync(Guid deviceId, string pin, DateTime attendanceTime);
}
