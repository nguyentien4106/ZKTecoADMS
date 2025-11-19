using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.Shifts.CreateShift;
using ZKTecoADMS.Application.Commands.Shifts.UpdateShift;
using ZKTecoADMS.Application.Commands.Shifts.DeleteShift;
using ZKTecoADMS.Application.Commands.Shifts.ApproveShift;
using ZKTecoADMS.Application.Commands.Shifts.RejectShift;
using ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee;
using ZKTecoADMS.Application.Queries.Shifts.GetPendingShifts;
using ZKTecoADMS.Application.Queries.Shifts.GetShiftsByManager;
using ZKTecoADMS.Api.Models.Responses;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController(IMediator mediator) : AuthenticatedControllerBase
{
    // Employee endpoints - can manage their own shifts
    [HttpGet("my-shifts")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<List<ShiftDto>>>> GetMyShifts()
    {
        // Assuming CurrentUserId links to an Employee
        var query = new GetShiftsByEmployeeQuery(CurrentUserId);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<ShiftDto>>> CreateShift([FromBody] CreateShiftRequest request)
    {
        var command = new CreateShiftCommand(
            request.EmployeeUserId ?? CurrentUserId,
            request.StartTime,
            request.EndTime,
            request.Description,
            IsManager);
        
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<ShiftDto>>> UpdateShift(Guid id, [FromBody] UpdateShiftRequest request)
    {
        var command = new UpdateShiftCommand(
            id,
            request.StartTime,
            request.EndTime,
            request.Description);
        
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<bool>>> DeleteShift(Guid id)
    {
        var command = new DeleteShiftCommand(id);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    // Manager endpoints - can view and approve/reject shifts
    [HttpGet("pending")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<List<ShiftDto>>>> GetPendingShifts()
    {
        var query = new GetPendingShiftsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("managed")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<List<ShiftDto>>>> GetManagedShifts()
    {
        var query = new GetShiftsByManagerQuery(CurrentUserId);
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

    [HttpPost("{id}/reject")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<ShiftDto>>> RejectShift(Guid id, [FromBody] RejectShiftRequest request)
    {
        var command = new RejectShiftCommand(id, CurrentUserId, request.RejectionReason);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}