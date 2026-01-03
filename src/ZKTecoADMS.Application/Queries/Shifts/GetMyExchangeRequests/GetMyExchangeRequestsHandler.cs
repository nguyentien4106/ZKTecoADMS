using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Shifts.GetMyExchangeRequests;

public class GetMyExchangeRequestsHandler(
    IRepository<ShiftExchangeRequest> exchangeRequestRepository
    ) : IQueryHandler<GetMyExchangeRequestsQuery, AppResponse<List<ShiftExchangeRequestDto>>>
{
    public async Task<AppResponse<List<ShiftExchangeRequestDto>>> Handle(GetMyExchangeRequestsQuery request, CancellationToken cancellationToken)
    {
        var query = exchangeRequestRepository.GetQuery()
            .Include(e => e.Shift)
            .Include(e => e.Requester)
            .Include(e => e.TargetEmployee)
            .AsQueryable();

        // Filter based on IncomingOnly flag
        if (request.IncomingOnly)
        {
            // Only show requests where current user is the target
            query = query.Where(e => e.TargetEmployeeId == request.EmployeeId && e.Status == ShiftExchangeStatus.Pending);
        }
        else
        {
            // Show all requests (sent or received)
            query = query.Where(e => e.RequesterId == request.EmployeeId || e.TargetEmployeeId == request.EmployeeId);
        }

        var exchangeRequests = await query
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);

        var dtos = exchangeRequests.Select(e => new ShiftExchangeRequestDto
        {
            Id = e.Id,
            ShiftId = e.ShiftId,
            Shift = e.Shift?.Adapt<ShiftDto>(),
            RequesterId = e.RequesterId,
            RequesterName = $"{e.Requester.FirstName} {e.Requester.LastName}",
            TargetEmployeeId = e.TargetEmployeeId,
            TargetEmployeeName = $"{e.TargetEmployee.FirstName} {e.TargetEmployee.LastName}",
            Reason = e.Reason,
            Status = e.Status.ToString(),
            RejectionReason = e.RejectionReason,
            RespondedAt = e.RespondedAt,
            CreatedAt = e.CreatedAt
        }).ToList();

        return AppResponse<List<ShiftExchangeRequestDto>>.Success(dtos);
    }
}
