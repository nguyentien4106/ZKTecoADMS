using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Domain.Repositories;

public interface IAttendanceRepository : IRepository<AttendanceLog>
{
    Task<IEnumerable<AttendanceLog>> GetByDeviceIdAsync(Guid deviceId, DateTime? startDate = null, DateTime? endDate = null);
    Task<IEnumerable<AttendanceLog>> GetByUserIdAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null);
    Task<IEnumerable<AttendanceLog>> GetUnprocessedLogsAsync();
    Task<bool> LogExistsAsync(Guid deviceId, string pin, DateTime attendanceTime);
}