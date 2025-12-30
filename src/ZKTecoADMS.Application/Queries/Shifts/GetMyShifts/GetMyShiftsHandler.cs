using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee.Strategy;

namespace ZKTecoADMS.Application.Queries.Shifts.GetMyShifts;

/// <summary>
/// Handler for getting employee shifts using strategy pattern based on employment type
/// </summary>
public class GetShiftsByEmployeeHandler : IQueryHandler<GetMyShiftsQuery, AppResponse<List<ShiftDto>>>
{
    private readonly ShiftStrategyFactory _strategyFactory;

    public GetShiftsByEmployeeHandler(
        IRepositoryPagedQuery<Shift> shiftRepository,
        IRepositoryPagedQuery<EmployeeBenefit> employeeBenefitRepository,
        IRepositoryPagedQuery<Holiday> holidayRepository,
        IRepositoryPagedQuery<Employee> employeeRepository)
    {
        _strategyFactory = new ShiftStrategyFactory(
            shiftRepository,
            employeeBenefitRepository,
            holidayRepository,
            employeeRepository);
    }

    public async Task<AppResponse<List<ShiftDto>>> Handle(
        GetMyShiftsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Create the appropriate strategy based on employee's salary rate type
            var (strategy, benefit, employee) = await _strategyFactory.CreateStrategyAsync(
                request.EmployeeId,
                cancellationToken);

            // Get shifts using the selected strategy
            var shifts = await strategy.GetShiftsAsync(
                employee,
                request.Month,
                request.Year,
                benefit,
                cancellationToken);

            // Filter by status if specified
            if (request.Status.HasValue)
            {
                shifts = shifts.Where(s => s.Status == request.Status.Value).ToList();
            }

            return AppResponse<List<ShiftDto>>.Success(shifts);
        }
        catch (ArgumentException argEx)
        {
            return AppResponse<List<ShiftDto>>.Fail($"Invalid argument: {argEx.Message}");
        }
        catch (InvalidOperationException invOpEx)
        {
            return AppResponse<List<ShiftDto>>.Fail($"{invOpEx.Message}");
        }
        catch (Exception ex)
        {
            return AppResponse<List<ShiftDto>>.Fail($"Failed to retrieve shifts: {ex.Message}");
        }
    }
}
