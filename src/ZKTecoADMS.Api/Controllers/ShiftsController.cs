using Mapster;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.Shifts.CreateShift;
using ZKTecoADMS.Application.Commands.Shifts.DeleteShift;
using ZKTecoADMS.Application.Commands.Shifts.ApproveShift;
using ZKTecoADMS.Application.Commands.Shifts.ApproveShifts;
using ZKTecoADMS.Application.Commands.Shifts.AssignShift;
using ZKTecoADMS.Application.Commands.Shifts.RejectShift;
using ZKTecoADMS.Application.Commands.Shifts.UpdateShift;
using ZKTecoADMS.Application.Queries.Shifts.GetPendingShifts;
using ZKTecoADMS.Application.Queries.Shifts.GetShiftsByManager;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Application.Queries.Shifts.GetMyShifts;

namespace ZKTecoADMS.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ShiftsController(IMediator mediator) : AuthenticatedControllerBase
{
    [HttpGet("my-shifts")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<List<ShiftDto>>>> GetMyShifts([FromQuery] int Month, [FromQuery] int Year, [FromQuery] ShiftStatus? Status)
    {
        var query = new GetMyShiftsQuery()
        {
            EmployeeId = EmployeeId,
            Month = Month,
            Year = Year,
            Status = Status
        };

        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.HourlyEmployeeOnly)]
    public async Task<ActionResult<AppResponse<ShiftDto>>> CreateShift([FromBody] CreateShiftRequest request)
    {
        var command = request.Adapt<CreateShiftCommand>();
        command.EmployeeId = EmployeeId;
        
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("assign")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<ShiftDto>>> AssignShift([FromBody] AssignShiftRequest request)
    {
        var command = request.Adapt<AssignShiftCommand>();
        
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyNames.HourlyEmployeeOnly)]
    public async Task<ActionResult<AppResponse<bool>>> DeleteShift(Guid id)
    {
        var command = new DeleteShiftCommand(id);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    // Manager endpoints - can view and approve/reject shifts
    [HttpGet("pending")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<PagedResult<ShiftDto>>>> GetPendingShifts([FromQuery]PaginationRequest request)
    {
        var query = new GetPendingShiftsQuery(CurrentUserId, request);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("managed")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<PagedResult<ShiftDto>>>> GetManagedShifts([FromQuery] PaginationRequest request, [FromBody]GetManagedShiftRequest filter)
    {
        var query = new GetShiftsByManagerQuery(CurrentUserId, request, filter);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{id}/approve")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<ShiftDto>>> ApproveShift(Guid id)
    {
        var command = new ApproveShiftCommand(id, CurrentUserId);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/approve-multiple")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<ShiftDto>>> ApproveShifts([FromBody] List<Guid> ids)
    {
        var command = new ApproveShiftsCommand(ids);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/reject")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<ShiftDto>>> RejectShift(Guid id, [FromBody] RejectShiftRequest request)
    {
        var command = new RejectShiftCommand(id, CurrentUserId, request.RejectionReason);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/times")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<ShiftDto>>> UpdateShiftTimes(Guid id, [FromBody] UpdateShiftTimesRequest request)
    {
        var command = new UpdateShiftCommand(id, CurrentUserId, request.CheckInTime, request.CheckOutTime);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}