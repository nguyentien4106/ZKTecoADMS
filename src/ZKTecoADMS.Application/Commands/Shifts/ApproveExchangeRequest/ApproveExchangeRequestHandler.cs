using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Exceptions;

namespace ZKTecoADMS.Application.Commands.Shifts.ApproveExchangeRequest;

public class ApproveExchangeRequestHandler(
    IRepository<ShiftExchangeRequest> exchangeRequestRepository,
    IRepository<Shift> shiftRepository
    ) : ICommandHandler<ApproveExchangeRequestCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(ApproveExchangeRequestCommand request, CancellationToken cancellationToken)
    {
        // Get the exchange request
        var exchangeRequest = await exchangeRequestRepository.GetQuery()
            .Include(e => e.Shift)
            .FirstOrDefaultAsync(e => e.Id == request.ExchangeRequestId, cancellationToken);

        if (exchangeRequest == null)
        {
            throw new NotFoundException(nameof(ShiftExchangeRequest), request.ExchangeRequestId);
        }

        // Verify the current user is the target employee
        if (exchangeRequest.TargetEmployeeId != request.TargetEmployeeId)
        {
            return AppResponse<bool>.Error("You are not authorized to approve this exchange request");
        }

        // Check if request is still pending
        if (exchangeRequest.Status != ShiftExchangeStatus.Pending)
        {
            return AppResponse<bool>.Error("This exchange request has already been processed");
        }

        // Update the shift to the new employee
        var shift = exchangeRequest.Shift;
        shift.EmployeeId = request.TargetEmployeeId;
        
        await shiftRepository.UpdateAsync(shift, cancellationToken);

        // Update the exchange request status
        exchangeRequest.Status = ShiftExchangeStatus.Approved;
        
        await exchangeRequestRepository.UpdateAsync(exchangeRequest, cancellationToken);

        return AppResponse<bool>.Success(true);
    }
}
