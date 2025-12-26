using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Repositories;

public class PayslipRepository(ZKTecoDbContext context) : IPayslipRepository
{
    public async Task<Payslip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Payslips
            .Include(p => p.EmployeeUser)
            .Include(p => p.SalaryProfile)
            .Include(p => p.GeneratedByUser)
            .Include(p => p.ApprovedByUser)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<Payslip>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Payslips
            .Include(p => p.EmployeeUser)
            .Include(p => p.SalaryProfile)
            .Include(p => p.GeneratedByUser)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .ThenBy(p => p.EmployeeUser.UserName)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Payslip>> GetByEmployeeUserIdAsync(Guid employeeUserId, CancellationToken cancellationToken = default)
    {
        return await context.Payslips
            .Include(p => p.SalaryProfile)
            .Include(p => p.GeneratedByUser)
            .Include(p => p.ApprovedByUser)
            .Where(p => p.EmployeeUserId == employeeUserId)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .ToListAsync(cancellationToken);
    }

    public async Task<Payslip?> GetByEmployeeUserAndPeriodAsync(Guid employeeUserId, int year, int month, CancellationToken cancellationToken = default)
    {
        return await context.Payslips
            .Include(p => p.EmployeeUser)
            .Include(p => p.SalaryProfile)
            .Include(p => p.GeneratedByUser)
            .Include(p => p.ApprovedByUser)
            .FirstOrDefaultAsync(p => p.EmployeeUserId == employeeUserId && p.Year == year && p.Month == month, cancellationToken);
    }

    public async Task<List<Payslip>> GetByPeriodAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        return await context.Payslips
            .Include(p => p.EmployeeUser)
            .Include(p => p.SalaryProfile)
            .Where(p => p.Year == year && p.Month == month)
            .OrderBy(p => p.EmployeeUser.UserName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Payslip> CreateAsync(Payslip payslip, CancellationToken cancellationToken = default)
    {
        context.Payslips.Add(payslip);
        await context.SaveChangesAsync(cancellationToken);
        return payslip;
    }

    public async Task<Payslip> UpdateAsync(Payslip payslip, CancellationToken cancellationToken = default)
    {
        context.Payslips.Update(payslip);
        await context.SaveChangesAsync(cancellationToken);
        return payslip;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payslip = await context.Payslips.FindAsync([id], cancellationToken);
        if (payslip != null)
        {
            context.Payslips.Remove(payslip);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsForEmployeeUserAndPeriodAsync(Guid employeeUserId, int year, int month, CancellationToken cancellationToken = default)
    {
        return await context.Payslips
            .AnyAsync(p => p.EmployeeUserId == employeeUserId && p.Year == year && p.Month == month, cancellationToken);
    }

    public async Task<List<Payslip>> GetPayslipsByManagerIdAsync(Guid managerId, int year, int month, CancellationToken cancellationToken = default) {
        return await context.Payslips
            .Include(p => p.EmployeeUser)
            .Include(p => p.SalaryProfile)
            .Where(p => p.EmployeeUser.ManagerId == managerId && p.Year == year && p.Month == month)
            .OrderBy(p => p.EmployeeUser.UserName)
            .ToListAsync(cancellationToken);
    }
}
