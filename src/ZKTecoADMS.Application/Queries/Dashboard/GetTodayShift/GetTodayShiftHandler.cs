using ZKTecoADMS.Application.DTOs.Dashboard;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Dashboard.GetTodayShift;

public class GetTodayShiftHandler(IRepository<Shift> shiftRepository)
    : IQueryHandler<GetTodayShiftQuery, AppResponse<ShiftInfoDto>>
{
    public async Task<AppResponse<ShiftInfoDto>> Handle(
        GetTodayShiftQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var shifts = await shiftRepository.GetAllAsync(cancellationToken: cancellationToken);
        
        var todayShift = shifts
            .Where(s => s.ApplicationUserId == request.UserId)
            .Where(s => s.StartTime.Date == today)
            .Where(s => s.Status == ShiftStatus.Approved)
            .OrderBy(s => s.StartTime)
            .FirstOrDefault();

        if (todayShift == null)
        {
            return AppResponse<ShiftInfoDto>.Success(null);
        }

        var dto = new ShiftInfoDto
        {
            Id = todayShift.Id,
            StartTime = todayShift.StartTime,
            EndTime = todayShift.EndTime,
            Description = todayShift.Description,
            Status = (int)todayShift.Status,
            TotalHours = (todayShift.EndTime - todayShift.StartTime).TotalHours,
            IsToday = true
        };

        return AppResponse<ShiftInfoDto>.Success(dto);
    }
}
