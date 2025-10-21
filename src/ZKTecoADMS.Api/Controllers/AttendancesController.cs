using Microsoft.AspNetCore.Mvc;
using ZKTecoADMS.Api.Models.Requests;
using ZKTecoADMS.Application.Commands.Attendances.GetAttendancesByDevices;
using ZKTecoADMS.Application.DTOs.Attendances;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Core.Services;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AttendancesController(
    IAttendanceService attendanceService, 
    ILogger<AttendancesController> logger,
    IMediator bus
    )
    : ControllerBase
{
    private readonly ILogger<AttendancesController> _logger = logger;

    
    [HttpPost("devices")]
    public async Task<ActionResult<AppResponse<PagedResult<AttendanceDto>>>> GetAttendanceByDevice(
        [FromQuery] PaginationRequest paginationRequest, [FromBody] GetAttendancesByDeviceRequest filter)
    {
        var command = new GetAttsByDevicesCommand(paginationRequest, filter);

        return Ok(await bus.Send(command));
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceByUser(
        Guid userId, 
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate)
    {
        var logs = await attendanceService.GetAttendanceByUserAsync(userId, startDate, endDate);
        return Ok(logs);
    }

}