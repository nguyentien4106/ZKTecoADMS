using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Exceptions;

namespace ZKTecoADMS.Application.Commands.Shifts.ExchangeShift;

public class ExchangeShiftHandler(
    IRepository<Shift> shiftRepository,
    IRepository<ShiftExchangeRequest> exchangeRequestRepository,
    IRepository<Employee> employeeRepository
    ) : ICommandHandler<ExchangeShiftCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(ExchangeShiftCommand request, CancellationToken cancellationToken)
    {
        // Validate shift exists and belongs to current employee
        var shift = await shiftRepository.GetByIdAsync(request.ShiftId, cancellationToken: cancellationToken);
        if (shift == null)
        {
            throw new NotFoundException(nameof(Shift), request.ShiftId);
        }

        if (shift.EmployeeId != request.CurrentEmployeeId)
        {
            return AppResponse<bool>.Error("You are not authorized to exchange this shift");
        }

        // Only approved shifts can be exchanged
        if (shift.Status != ShiftStatus.Approved)
        {
            return AppResponse<bool>.Error("Only approved shifts can be exchanged");
        }

        // Validate target employee exists
        var targetEmployee = await employeeRepository.GetByIdAsync(request.TargetEmployeeId, cancellationToken: cancellationToken);
        if (targetEmployee == null)
        {
            throw new NotFoundException(nameof(Employee), request.TargetEmployeeId);
        }

        // Check if requester is trying to exchange with themselves
        if (request.CurrentEmployeeId == request.TargetEmployeeId)
        {
            return AppResponse<bool>.Error("Cannot exchange shift with yourself");
        }

        // Check if there's already a pending exchange request for this shift
        var existingRequest = await exchangeRequestRepository.GetQuery()
            .Where(e => e.ShiftId == request.ShiftId && e.Status == ShiftExchangeStatus.Pending)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingRequest != null)
        {
            return AppResponse<bool>.Error("There is already a pending exchange request for this shift");
        }

        // Create the exchange request
        var exchangeRequest = new ShiftExchangeRequest
        {
            Id = Guid.NewGuid(),
            ShiftId = request.ShiftId,
            RequesterId = request.CurrentEmployeeId,
            TargetEmployeeId = request.TargetEmployeeId,
            Reason = request.Reason,
            Status = ShiftExchangeStatus.Pending,
        };

        await exchangeRequestRepository.AddAsync(exchangeRequest, cancellationToken);

        return AppResponse<bool>.Success(true);
    }
}