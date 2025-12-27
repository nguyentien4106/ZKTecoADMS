using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Repositories;

public class EmployeeSalaryProfileRepository : EfRepository<EmployeeSalaryProfile>, IEmployeeSalaryProfileRepository
{
    private readonly ZKTecoDbContext _context;

    public EmployeeSalaryProfileRepository(ZKTecoDbContext context, ILogger<EfRepository<EmployeeSalaryProfile>> logger) 
        : base(context, logger)
    {
        _context = context;
    }

    public async Task<EmployeeSalaryProfile?> GetActiveByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<EmployeeSalaryProfile>()
            .Include(x => x.Employee)
            .Where(x => x.EmployeeId == employeeId && x.IsActive)
            .OrderByDescending(x => x.EffectiveDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<EmployeeSalaryProfile>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<EmployeeSalaryProfile>()
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.EffectiveDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeSalaryProfile?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<EmployeeSalaryProfile>()
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task DeactivateOtherProfilesAsync(Guid employeeId, Guid currentProfileId, CancellationToken cancellationToken = default)
    {
        var otherProfiles = await _context.Set<EmployeeSalaryProfile>()
            .Where(x => x.EmployeeId == employeeId && x.Id != currentProfileId && x.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var profile in otherProfiles)
        {
            profile.IsActive = false;
            profile.EndDate = DateTime.Now;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
