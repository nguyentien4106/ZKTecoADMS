using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Api.Models.Responses;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Dashboard;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.Dashboard.GetManagerDashboard;
using ZKTecoADMS.Application.Queries.Dashboard.GetEmployeeDashboard;
using ZKTecoADMS.Application.Queries.Dashboard.GetTodayShift;
using ZKTecoADMS.Application.Queries.Dashboard.GetNextShift;
using ZKTecoADMS.Application.Queries.Dashboard.GetCurrentAttendance;
using ZKTecoADMS.Application.Queries.Dashboard.GetAttendanceStats;

namespace ZKTecoADMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(
    IMediator mediator,
    ILogger<DashboardController> logger
) : AuthenticatedControllerBase
{
    /// <summary>
    /// Get manager dashboard with core information
    /// </summary>
    /// <param name="date">Date for the dashboard data (optional, defaults to today)</param>
    /// <returns>Dashboard data with employees on leave, absent, late, and attendance rate</returns>
    [HttpGet("manager")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    [ProducesResponseType(typeof(AppResponse<ManagerDashboardDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AppResponse<ManagerDashboardDto>>> GetManagerDashboard(
        [FromQuery] DateTime? date = null)
    {
        try
        {
            var targetDate = date ?? DateTime.Today;

            var query = new GetManagerDashboardQuery(
                CurrentUserId,
                targetDate
            );

            var result = await mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving manager dashboard data");
            return StatusCode(500, AppResponse<ManagerDashboardDto>.Fail("An error occurred while retrieving manager dashboard data"));
        }
    }

    // Employee Dashboard Endpoints

    /// <summary>
    /// Get complete employee dashboard data
    /// </summary>
    /// <param name="period">Period for attendance stats (week, month, year)</param>
    [HttpGet("employee")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    [ProducesResponseType(typeof(AppResponse<EmployeeDashboardDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<EmployeeDashboardDto>>> GetEmployeeDashboard([FromQuery] string period = "week")
    {
        try
        {
            var query = new GetEmployeeDashboardQuery(CurrentUserId, period);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving employee dashboard");
            return StatusCode(500, AppResponse<EmployeeDashboardDto>.Fail("An error occurred while retrieving employee dashboard"));
        }
    }

    /// <summary>
    /// Get Current Shift information
    /// </summary>
    [HttpGet("shifts/today")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    [ProducesResponseType(typeof(AppResponse<ShiftInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<ShiftInfoDto>>> GetTodayShift()
    {
        try
        {
            var query = new GetTodayShiftQuery(CurrentUserId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving Current Shift");
            return StatusCode(500, AppResponse<ShiftInfoDto>.Fail("An error occurred while retrieving Current Shift"));
        }
    }

    /// <summary>
    /// Get next upcoming shift
    /// </summary>
    [HttpGet("shifts/next")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    [ProducesResponseType(typeof(AppResponse<ShiftInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<ShiftInfoDto>>> GetNextShift()
    {
        try
        {
            var query = new GetNextShiftQuery(CurrentUserId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving next shift");
            return StatusCode(500, AppResponse<ShiftInfoDto>.Fail("An error occurred while retrieving next shift"));
        }
    }

    /// <summary>
    /// Get current day attendance status
    /// </summary>
    [HttpGet("attendance/current")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    [ProducesResponseType(typeof(AppResponse<AttendanceInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<AttendanceInfoDto>>> GetCurrentAttendance()
    {
        try
        {
            var query = new GetCurrentAttendanceQuery(CurrentUserId);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving current attendance");
            return StatusCode(500, AppResponse<AttendanceInfoDto>.Fail("An error occurred while retrieving current attendance"));
        }
    }

    /// <summary>
    /// Get attendance statistics for a period
    /// </summary>
    /// <param name="period">Period for stats (week, month, year)</param>
    [HttpGet("attendance/stats")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    [ProducesResponseType(typeof(AppResponse<AttendanceStatsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<AttendanceStatsDto>>> GetAttendanceStats([FromQuery] string period = "week")
    {
        try
        {
            var query = new GetAttendanceStatsQuery(CurrentUserId, period);
            var result = await mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving attendance stats");
            return StatusCode(500, AppResponse<AttendanceStatsDto>.Fail("An error occurred while retrieving attendance stats"));
        }
    }
}
