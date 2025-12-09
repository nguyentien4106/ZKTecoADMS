using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Repositories;

public class SalaryProfileRepository : EfRepository<SalaryProfile>, ISalaryProfileRepository
{
    private readonly ZKTecoDbContext _context;

    public SalaryProfileRepository(ZKTecoDbContext context, ILogger<EfRepository<SalaryProfile>> logger) 
        : base(context, logger)
    {
        _context = context;
    }

    public async Task<SalaryProfile?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<SalaryProfile>()
            .Include(x => x.EmployeeSalaryProfiles)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<SalaryProfile>> GetActiveProfilesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<SalaryProfile>()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<SalaryProfile>()
            .Where(x => x.Name.ToLower() == name.ToLower());

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }
}
