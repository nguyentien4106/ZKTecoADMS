using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Queries.Attendances.GetAttendancesByDevices;
using ZKTecoADMS.Application.DTOs.Attendances;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AttendancesController(
    IAttendanceService attendanceService, 
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
        var logs = await attendanceService.GetAttendanceByUserAsync(deviceId, userId, startDate, endDate);
        return Ok(logs);
    }

}