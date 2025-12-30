using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee.Strategy;

/// <summary>
/// Strategy interface for retrieving employee shifts based on employment type
/// </summary>
public interface IGetShiftsStrategy
{
    /// <summary>
    /// Gets shifts for an employee for a specific month and year
    /// </summary>
    /// <param name="employeeUserId">The employee's user ID</param>
    /// <param name="month">The month to retrieve shifts for</param>
    /// <param name="year">The year to retrieve shifts for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of shifts for the specified period</returns>
    Task<List<ShiftDto>> GetShiftsAsync(
        Employee employee, 
        int month, 
        int year,
        Benefit employeeBenefit,
        CancellationToken cancellationToken = default
    );
}
