using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Queries.Attendances.GetAttendancesByDevices;
using ZKTecoADMS.Application.Queries.Attendances.GetMonthlyAttendanceSummary;
using ZKTecoADMS.Application.DTOs.Attendances;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AttendancesController(
    IMediator bus
    )
    : AuthenticatedControllerBase
{
    [HttpPost("devices")]
    public async Task<ActionResult<AppResponse<PagedResult<AttendanceDto>>>> GetAttendanceByDevice(
        [FromQuery] PaginationRequest paginationRequest, [FromBody] GetAttendancesByDeviceRequest filter)
    {
        var command = new GetAttsByDevicesQuery(paginationRequest, filter);

        return Ok(await bus.Send(command));
    }

    [HttpGet("devices/{deviceId}/users/{userId}")]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceByUser(
        Guid deviceId,
        Guid userId, 
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate)
    {
        return Ok(null);
    }

    [HttpPost("monthly-summary")]
    public async Task<ActionResult<AppResponse<MonthlyAttendanceSummaryDto>>> GetMonthlyAttendanceSummary(
        [FromQuery] int year,
        [FromQuery] int month,
        [FromBody] GetMonthlyAttendanceSummaryRequest request)
    {
        var query = new GetMonthlyAttendanceSummaryQuery(request.EmployeeIds, year, month);
        return Ok(await bus.Send(query));
    }

}

public class GetMonthlyAttendanceSummaryRequest
{
    public List<Guid> EmployeeIds { get; set; } = [];
}