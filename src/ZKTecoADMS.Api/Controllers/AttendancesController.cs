using Microsoft.AspNetCore.Mvc;
using ZKTecoADMS.Api.Models.Requests;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Core.Services;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AttendancesController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    private readonly ILogger<AttendancesController> _logger;

    public AttendancesController(IAttendanceService attendanceService, ILogger<AttendancesController> logger)
    {
        _attendanceService = attendanceService;
        _logger = logger;
    }

    [HttpGet("device/{deviceId}")]
    public async Task<ActionResult<IEnumerable<AttendanceLog>>> GetAttendanceByDevice(
        Guid deviceId, 
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate)
    {
        var logs = await _attendanceService.GetAttendanceByDeviceAsync(deviceId, startDate, endDate);
        return Ok(logs);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<AttendanceLog>>> GetAttendanceByUser(
        Guid userId, 
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate)
    {
        var logs = await _attendanceService.GetAttendanceByUserAsync(userId, startDate, endDate);
        return Ok(logs);
    }

    [HttpGet("unprocessed")]
    public async Task<ActionResult<IEnumerable<AttendanceLog>>> GetUnprocessedLogs()
    {
        var logs = await _attendanceService.GetUnprocessedLogsAsync();
        return Ok(logs);
    }

    [HttpPost("mark-processed")]
    public async Task<IActionResult> MarkLogsAsProcessed([FromBody] MarkProcessedRequest request)
    {
        await _attendanceService.MarkLogsAsProcessedAsync(request.LogIds);
        return Ok(new { message = $"{request.LogIds.Count} logs marked as processed" });
    }
}