using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Domain.Repositories;

public interface IPayslipRepository
{
    Task<Payslip?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Payslip>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Payslip>> GetByEmployeeUserIdAsync(Guid employeeUserId, CancellationToken cancellationToken = default);
    Task<Payslip?> GetByEmployeeUserAndPeriodAsync(Guid employeeUserId, int year, int month, CancellationToken cancellationToken = default);
    Task<List<Payslip>> GetByPeriodAsync(int year, int month, CancellationToken cancellationToken = default);
    Task<Payslip> CreateAsync(Payslip payslip, CancellationToken cancellationToken = default);
    Task<Payslip> UpdateAsync(Payslip payslip, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsForEmployeeUserAndPeriodAsync(Guid employeeUserId, int year, int month, CancellationToken cancellationToken = default);
    Task<List<Payslip>> GetPayslipsByManagerIdAsync(Guid managerId, int year, int month, CancellationToken cancellationToken = default);
}
