using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.Attendances;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Attendances.GetAttendancesByDevices;

public class GetAttsByDevicesHandler(IRepositoryPagedQuery<Attendance> attRepository) : ICommandHandler<GetAttsByDevicesQuery, AppResponse<PagedResult<AttendanceDto>>>
{
    public async Task<AppResponse<PagedResult<AttendanceDto>>> Handle(GetAttsByDevicesQuery request, CancellationToken cancellationToken)
    {
        var atts = await attRepository.GetPagedResultWithProjectionAsync(
            request.PaginationRequest,
            filter: a => 
                a.AttendanceTime.Date <= request.Filter.ToDate.Date
                && a.AttendanceTime.Date >= request.Filter.FromDate.Date
                && request.Filter.DeviceIds.Contains(a.DeviceId),
            projection: a => new AttendanceDto(
                a.Id,
                a.AttendanceTime,
                a.Device.DeviceName,      // Only loads DeviceName field
                a.User.Name ?? string.Empty,  // Only loads Name field
                a.VerifyMode,
                a.AttendanceState,
                a.WorkCode
            ),
            cancellationToken: cancellationToken);
        
        return AppResponse<PagedResult<AttendanceDto>>.Success(atts);
    }
}