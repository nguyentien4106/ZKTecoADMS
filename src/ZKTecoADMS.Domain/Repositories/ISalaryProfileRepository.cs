using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Domain.Repositories;

public interface ISalaryProfileRepository : IRepository<SalaryProfile>
{
    Task<SalaryProfile?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<SalaryProfile>> GetActiveProfilesAsync(CancellationToken cancellationToken = default);
    Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
