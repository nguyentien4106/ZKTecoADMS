using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Api.Models.Responses;
using ZKTecoADMS.Application.DTOs.Dashboard;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.Dashboard.GetDashboardData;

namespace ZKTecoADMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(
    IMediator mediator,
    ILogger<DashboardController> logger
) : AuthenticatedControllerBase
{
    /// <summary>
    /// Get comprehensive dashboard data for managers to monitor employee performance
    /// </summary>
    /// <param name="startDate">Start date for the analysis period (optional, defaults to 30 days ago)</param>
    /// <param name="endDate">End date for the analysis period (optional, defaults to today)</param>
    /// <param name="department">Filter by department (optional)</param>
    /// <param name="topPerformersCount">Number of top performers to return (default: 10)</param>
    /// <param name="lateEmployeesCount">Number of late employees to return (default: 10)</param>
    /// <param name="trendDays">Number of days for trend analysis (default: 30)</param>
    /// <returns>Dashboard data with summary, performance metrics, trends, and device status</returns>
    [HttpGet]
    [ProducesResponseType(typeof(AppResponse<DashboardDataDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<DashboardDataDto>>> GetDashboardData(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? department = null,
        [FromQuery] int topPerformersCount = 10,
        [FromQuery] int lateEmployeesCount = 10,
        [FromQuery] int trendDays = 30)
    {
        try
        {
            var end = endDate ?? DateTime.Today;
            var start = startDate ?? end.AddDays(-30);

            if (start > end)
            {
                return BadRequest(AppResponse<DashboardDataDto>.Fail("Start date cannot be after end date"));
            }

            if (topPerformersCount < 1 || topPerformersCount > 100)
            {
                return BadRequest(AppResponse<DashboardDataDto>.Fail("Top performers count must be between 1 and 100"));
            }

            if (lateEmployeesCount < 1 || lateEmployeesCount > 100)
            {
                return BadRequest(AppResponse<DashboardDataDto>.Fail("Late employees count must be between 1 and 100"));
            }

            if (trendDays < 1 || trendDays > 90)
            {
                return BadRequest(AppResponse<DashboardDataDto>.Fail("Trend days must be between 1 and 90"));
            }

            var query = new GetDashboardDataQuery(
                start,
                end,
                CurrentUserId,
                department,
                topPerformersCount,
                lateEmployeesCount,
                trendDays
            );

            var result = await mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving dashboard data");
            return StatusCode(500, AppResponse<DashboardDataDto>.Fail("An error occurred while retrieving dashboard data"));
        }
    }

    /// <summary>
    /// Get summary statistics for today
    /// </summary>
    /// <returns>Dashboard summary with key metrics</returns>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(AppResponse<DashboardSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<DashboardSummaryDto>>> GetTodaySummary()
    {
        try
        {
            var today = DateTime.Today;
            var query = new GetDashboardDataQuery(
                today,
                today,
                CurrentUserId,
                null,
                0,
                0,
                1
            );

            var result = await mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(AppResponse<DashboardSummaryDto>.Success(result.Data!.Summary));
            }

            return BadRequest(AppResponse<DashboardSummaryDto>.Fail(result.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving today's summary");
            return StatusCode(500, AppResponse<DashboardSummaryDto>.Fail("An error occurred while retrieving summary data"));
        }
    }

    /// <summary>
    /// Get top performing employees
    /// </summary>
    /// <param name="startDate">Start date for the analysis period (optional, defaults to 30 days ago)</param>
    /// <param name="endDate">End date for the analysis period (optional, defaults to today)</param>
    /// <param name="count">Number of top performers to return (default: 10)</param>
    /// <param name="department">Filter by department (optional)</param>
    /// <returns>List of top performing employees</returns>
    [HttpGet("top-performers")]
    [ProducesResponseType(typeof(AppResponse<List<EmployeePerformanceDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<List<EmployeePerformanceDto>>>> GetTopPerformers(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int count = 10,
        [FromQuery] string? department = null)
    {
        try
        {
            var end = endDate ?? DateTime.Today;
            var start = startDate ?? end.AddDays(-30);

            var query = new GetDashboardDataQuery(
                start,
                end,
                CurrentUserId,
                department,
                count,
                0,
                0
            );

            var result = await mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(AppResponse<List<EmployeePerformanceDto>>.Success(result.Data!.TopPerformers));
            }

            return BadRequest(AppResponse<List<EmployeePerformanceDto>>.Fail(result.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving top performers");
            return StatusCode(500, AppResponse<List<EmployeePerformanceDto>>.Fail("An error occurred while retrieving top performers"));
        }
    }

    /// <summary>
    /// Get employees with frequent late arrivals
    /// </summary>
    /// <param name="startDate">Start date for the analysis period (optional, defaults to 30 days ago)</param>
    /// <param name="endDate">End date for the analysis period (optional, defaults to today)</param>
    /// <param name="count">Number of late employees to return (default: 10)</param>
    /// <param name="department">Filter by department (optional)</param>
    /// <returns>List of employees with late arrival issues</returns>
    [HttpGet("late-employees")]
    [ProducesResponseType(typeof(AppResponse<List<EmployeePerformanceDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<List<EmployeePerformanceDto>>>> GetLateEmployees(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int count = 10,
        [FromQuery] string? department = null)
    {
        try
        {
            var end = endDate ?? DateTime.Today;
            var start = startDate ?? end.AddDays(-30);

            var query = new GetDashboardDataQuery(
                start,
                end,
                CurrentUserId,
                department,
                0,
                count,
                0
            );

            var result = await mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(AppResponse<List<EmployeePerformanceDto>>.Success(result.Data!.LateEmployees));
            }

            return BadRequest(AppResponse<List<EmployeePerformanceDto>>.Fail(result.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving late employees");
            return StatusCode(500, AppResponse<List<EmployeePerformanceDto>>.Fail("An error occurred while retrieving late employees"));
        }
    }

    /// <summary>
    /// Get attendance statistics by department
    /// </summary>
    /// <param name="startDate">Start date for the analysis period (optional, defaults to 30 days ago)</param>
    /// <param name="endDate">End date for the analysis period (optional, defaults to today)</param>
    /// <returns>List of department statistics</returns>
    [HttpGet("department-stats")]
    [ProducesResponseType(typeof(AppResponse<List<DepartmentStatisticsDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<List<DepartmentStatisticsDto>>>> GetDepartmentStatistics(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var end = endDate ?? DateTime.Today;
            var start = startDate ?? end.AddDays(-30);

            var query = new GetDashboardDataQuery(
                start,
                end,
                CurrentUserId,
                null,
                0,
                0,
                0
            );

            var result = await mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(AppResponse<List<DepartmentStatisticsDto>>.Success(result.Data!.DepartmentStats));
            }

            return BadRequest(AppResponse<List<DepartmentStatisticsDto>>.Fail(result.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving department statistics");
            return StatusCode(500, AppResponse<List<DepartmentStatisticsDto>>.Fail("An error occurred while retrieving department statistics"));
        }
    }

    /// <summary>
    /// Get attendance trends over time
    /// </summary>
    /// <param name="days">Number of days for trend analysis (default: 30, max: 90)</param>
    /// <returns>List of attendance trends by date</returns>
    [HttpGet("attendance-trends")]
    [ProducesResponseType(typeof(AppResponse<List<AttendanceTrendDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<List<AttendanceTrendDto>>>> GetAttendanceTrends(
        [FromQuery] int days = 30)
    {
        try
        {
            if (days < 1 || days > 90)
            {
                return BadRequest(AppResponse<List<AttendanceTrendDto>>.Fail("Days must be between 1 and 90"));
            }

            var end = DateTime.Today;
            var start = end.AddDays(-days + 1);

            var query = new GetDashboardDataQuery(
                start,
                end,
                CurrentUserId,
                null,
                0,
                0,
                days
            );

            var result = await mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(AppResponse<List<AttendanceTrendDto>>.Success(result.Data!.AttendanceTrends));
            }

            return BadRequest(AppResponse<List<AttendanceTrendDto>>.Fail(result.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving attendance trends");
            return StatusCode(500, AppResponse<List<AttendanceTrendDto>>.Fail("An error occurred while retrieving attendance trends"));
        }
    }

    /// <summary>
    /// Get device status information
    /// </summary>
    /// <returns>List of devices with their current status and usage</returns>
    [HttpGet("device-status")]
    [ProducesResponseType(typeof(AppResponse<List<DeviceStatusDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AppResponse<List<DeviceStatusDto>>>> GetDeviceStatus()
    {
        try
        {
            var today = DateTime.Today;
            var query = new GetDashboardDataQuery(
                today,
                today,
                CurrentUserId,
                null,
                0,
                0,
                1
            );

            var result = await mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(AppResponse<List<DeviceStatusDto>>.Success(result.Data!.DeviceStatuses));
            }

            return BadRequest(AppResponse<List<DeviceStatusDto>>.Fail(result.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving device status");
            return StatusCode(500, AppResponse<List<DeviceStatusDto>>.Fail("An error occurred while retrieving device status"));
        }
    }
}
