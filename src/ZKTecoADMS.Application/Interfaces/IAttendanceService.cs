using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

public interface IAttendanceService 
{
    Task SaveAttendanceLogsAsync(IEnumerable<AttendanceLog> logs);
    Task<IEnumerable<AttendanceLog>> GetAttendanceByDeviceAsync(Guid deviceId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<AttendanceLog>> GetAttendanceByUserAsync(Guid userId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<AttendanceLog>> GetUnprocessedLogsAsync();
    Task MarkLogsAsProcessedAsync(IEnumerable<Guid> logIds);
}
