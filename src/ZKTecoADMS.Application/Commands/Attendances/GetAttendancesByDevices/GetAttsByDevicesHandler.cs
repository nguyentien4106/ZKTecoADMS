using ZKTecoADMS.Application.DTOs.Attendances;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.Attendances.GetAttendancesByDevices;

public class GetAttsByDevicesHandler(IRepositoryPagedQuery<Attendance> attRepository) : ICommandHandler<GetAttsByDevicesCommand, AppResponse<PagedResult<AttendanceDto>>>
{
    public async Task<AppResponse<PagedResult<AttendanceDto>>> Handle(GetAttsByDevicesCommand request, CancellationToken cancellationToken)
    {
        var atts = await attRepository.GetPagedResultAsync(
            request.PaginationRequest,
            a => 
                a.AttendanceTime.Date <= request.Filter.ToDate 
                && a.AttendanceTime.Date >= request.Filter.FromDate 
                && request.Filter.DeviceIds.Contains(a.DeviceId), 
            includeProperties: ["Device", "User"],
            cancellationToken: cancellationToken);
        
        return AppResponse<PagedResult<AttendanceDto>>.Success(new PagedResult<AttendanceDto>(atts.Items.Adapt<IEnumerable<AttendanceDto>>(), atts.TotalCount, atts.PageNumber, atts.PageSize));
    }
}