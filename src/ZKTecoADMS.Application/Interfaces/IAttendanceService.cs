using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

public interface IAttendanceService 
{
    Task SaveAttendanceLogsAsync(IEnumerable<Attendance> logs);
    Task<IEnumerable<Attendance>> GetAttendanceByDeviceAsync(Guid deviceId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<Attendance>> GetAttendanceByUserAsync(Guid userId, DateTime? startDate, DateTime? endDate);
}
