using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Exceptions;

namespace ZKTecoADMS.Application.Commands.Shifts.RejectExchangeRequest;

public class RejectExchangeRequestHandler(
    IRepository<ShiftExchangeRequest> exchangeRequestRepository
    ) : ICommandHandler<RejectExchangeRequestCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(RejectExchangeRequestCommand request, CancellationToken cancellationToken)
    {
        // Get the exchange request
        var exchangeRequest = await exchangeRequestRepository.GetQuery()
            .FirstOrDefaultAsync(e => e.Id == request.ExchangeRequestId, cancellationToken);

        if (exchangeRequest == null)
        {
            throw new NotFoundException(nameof(ShiftExchangeRequest), request.ExchangeRequestId);
        }

        // Verify the current user is the target employee
        if (exchangeRequest.TargetEmployeeId != request.TargetEmployeeId)
        {
            return AppResponse<bool>.Error("You are not authorized to reject this exchange request");
        }

        // Check if request is still pending
        if (exchangeRequest.Status != ShiftExchangeStatus.Pending)
        {
            return AppResponse<bool>.Error("This exchange request has already been processed");
        }

        // Update the exchange request status
        exchangeRequest.Status = ShiftExchangeStatus.Rejected;
        exchangeRequest.RejectionReason = request.RejectionReason;
        
        await exchangeRequestRepository.UpdateAsync(exchangeRequest, cancellationToken);

        return AppResponse<bool>.Success(true);
    }
}
