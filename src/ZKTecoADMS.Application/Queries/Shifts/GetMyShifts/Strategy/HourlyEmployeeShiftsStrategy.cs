using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee.Strategy;

/// <summary>
/// Strategy for getting shifts for hourly employees.
/// For hourly employees, shifts must be registered/requested by the employee themselves.
/// Only returns shifts that exist in the database.
/// </summary>
public class HourlyEmployeeShiftsStrategy : IGetShiftsStrategy
{
    private readonly IRepositoryPagedQuery<Shift> _shiftRepository;
    private readonly IRepositoryPagedQuery<Employee> _employeeRepository;

    public HourlyEmployeeShiftsStrategy(
        IRepositoryPagedQuery<Shift> shiftRepository,
        IRepositoryPagedQuery<Employee> employeeRepository)
    {
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<List<ShiftDto>> GetShiftsAsync(
        Employee employee,
        int month,
        int year,
        Benefit benefit,
        CancellationToken cancellationToken = default)
    {
        // Get employee information
        var shifts = await _shiftRepository.GetAllAsync(
            filter: s => s.EmployeeId == employee.Id && s.StartTime.Month == month && s.StartTime.Year == year,
            cancellationToken: cancellationToken);

        return shifts.Select(shift => new ShiftDto
        {
            Id = shift.Id,
            EmployeeId = shift.EmployeeId,
            EmployeeName = employee.GetFullName(),
            EmployeeCode = employee.EmployeeCode,
            Date = shift.StartTime.Date,
            StartTime = TimeOnly.FromDateTime(shift.StartTime),
            EndTime = TimeOnly.FromDateTime(shift.EndTime),
            Status = shift.Status,
            BreakTimeMinutes = shift.BreakTimeMinutes,
        }).ToList();
    }
}
