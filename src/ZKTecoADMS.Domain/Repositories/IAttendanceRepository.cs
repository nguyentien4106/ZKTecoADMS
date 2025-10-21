using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Domain.Repositories;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<IEnumerable<Attendance>> GetByDeviceIdAsync(Guid deviceId, DateTime? startDate = null, DateTime? endDate = null);
    Task<IEnumerable<Attendance>> GetByUserIdAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null);
    Task<bool> LogExistsAsync(Guid deviceId, string pin, DateTime attendanceTime);
}