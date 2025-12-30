using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee.Strategy;

/// <summary>
/// Factory for creating the appropriate shift retrieval strategy based on employee's salary rate type
/// </summary>
public class ShiftStrategyFactory
{
    private readonly IRepositoryPagedQuery<Shift> _shiftRepository;
    private readonly IRepositoryPagedQuery<EmployeeBenefit> _employeeBenefitRepository;
    private readonly IRepositoryPagedQuery<Holiday> _holidayRepository;
    private readonly IRepositoryPagedQuery<Employee> _employeeRepository;

    public ShiftStrategyFactory(
        IRepositoryPagedQuery<Shift> shiftRepository,
        IRepositoryPagedQuery<EmployeeBenefit> employeeBenefitRepository,
        IRepositoryPagedQuery<Holiday> holidayRepository,
        IRepositoryPagedQuery<Employee> employeeRepository)
    {
        _shiftRepository = shiftRepository;
        _employeeBenefitRepository = employeeBenefitRepository;
        _holidayRepository = holidayRepository;
        _employeeRepository = employeeRepository;
    }

    /// <summary>
    /// Creates the appropriate strategy based on the employee's benefit rate type
    /// </summary>
    /// <param name="employeeUserId">The employee's user ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The appropriate shift retrieval strategy</returns>
    public async Task<(IGetShiftsStrategy, Benefit, Employee)> CreateStrategyAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        // Get employee
        var employee = await _employeeRepository.GetByIdAsync(
            employeeId,
            cancellationToken: cancellationToken);

        if (employee == null)
        {
            throw new ArgumentException("Employee not found", nameof(employeeId));
        }

        // Get active employee benefit to determine rate type
        var employeeBenefit = await _employeeBenefitRepository.GetSingleAsync(
            filter: eb => eb.EmployeeId == employee.Id && eb.IsActive,
            includeProperties: [nameof(EmployeeBenefit.Benefit)],
            cancellationToken: cancellationToken);

        if( employeeBenefit == null)
        {
            throw new InvalidOperationException("You are not configured to get shifts because your manager has not set up your benefits");
        }

        // Determine strategy based on rate type
        if (employeeBenefit?.Benefit.RateType == SalaryRateType.Monthly)
        {
            return (
                new MonthlyEmployeeShiftsStrategy(
                _shiftRepository,
                _employeeBenefitRepository,
                _holidayRepository,
                _employeeRepository),
                employeeBenefit.Benefit,
                employee
            );
        }

        // Default to hourly strategy
        return (
            new HourlyEmployeeShiftsStrategy(_shiftRepository, _employeeRepository),
            employeeBenefit.Benefit,
            employee
        );
    }
}
