using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Domain.Repositories;

public interface IEmployeeSalaryProfileRepository : IRepository<EmployeeSalaryProfile>
{
    Task<EmployeeSalaryProfile?> GetActiveByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<List<EmployeeSalaryProfile>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<EmployeeSalaryProfile?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeactivateOtherProfilesAsync(Guid employeeId, Guid currentProfileId, CancellationToken cancellationToken = default);
}
