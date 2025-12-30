using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee.Strategy;

/// <summary>
/// Strategy for getting shifts for monthly employees.
/// For monthly employees, shifts are automatically generated for each working day in the month,
/// excluding days specified in WeeklyOffDays from the employee's benefit profile.
/// </summary>
public class MonthlyEmployeeShiftsStrategy : IGetShiftsStrategy
{
    private readonly IRepositoryPagedQuery<Shift> _shiftRepository;
    private readonly IRepositoryPagedQuery<EmployeeBenefit> _employeeBenefitRepository;
    private readonly IRepositoryPagedQuery<Holiday> _holidayRepository;
    private readonly IRepositoryPagedQuery<Employee> _employeeRepository;

    public MonthlyEmployeeShiftsStrategy(
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

    public async Task<List<ShiftDto>> GetShiftsAsync(
        Employee employee,
        int month,
        int year,
        Benefit benefit,
        CancellationToken cancellationToken = default)
    {
        // Get employee to access their ID
        var businessDays = new List<DateTime>();

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1).AddDays(-1);
        var weeklyOffDays = benefit.WeeklyOffDays?.Split(",").Select(d => Enum.Parse<DayOfWeek>(d)) ?? [];

        for (var date = start; date <= end; date = date.AddDays(1))
        {
            if (!weeklyOffDays.Contains(date.DayOfWeek))
            {
                businessDays.Add(date);
            }
        }

        // Get existing shifts from the database
        return businessDays.Select(d => new ShiftDto
        {
            Id = Guid.NewGuid(),
            EmployeeId = employee.Id,
            EmployeeName = employee.GetFullName(),
            EmployeeCode = employee.EmployeeCode,
            Date = d,
            StartTime = benefit.CheckIn.Value, // Assuming a 9 AM start time
            EndTime = benefit.CheckOut.Value, // Assuming a 6 PM end time
            Status = ShiftStatus.Approved
        }).ToList();
    }

}
