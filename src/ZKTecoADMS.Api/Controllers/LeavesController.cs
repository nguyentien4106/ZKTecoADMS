using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.Leaves.CreateLeave;
using ZKTecoADMS.Application.Commands.Leaves.CancelLeave;
using ZKTecoADMS.Application.Commands.Leaves.ApproveLeave;
using ZKTecoADMS.Application.Commands.Leaves.RejectLeave;
using ZKTecoADMS.Application.Queries.Leaves.GetMyLeaves;
using ZKTecoADMS.Application.Queries.Leaves.GetPendingLeaves;
using ZKTecoADMS.Application.Queries.Leaves.GetAllLeaves;
using ZKTecoADMS.Api.Models.Responses;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeavesController(IMediator mediator) : AuthenticatedControllerBase
{
    // Employee endpoints - can manage their own leaves
    [HttpGet("my-leaves")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<List<LeaveDto>>>> GetMyLeaves()
    {
        var query = new GetMyLeavesQuery(CurrentUserId);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<LeaveDto>>> CreateLeave([FromBody] CreateLeaveRequest request)
    {
        var command = new CreateLeaveCommand(
            CurrentUserId,
            request.Type,
            request.StartDate,
            request.EndDate,
            request.IsFullDay,
            request.Reason);
        
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<bool>>> CancelLeave(Guid id)
    {
        var command = new CancelLeaveCommand(id, CurrentUserId);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    // Manager endpoints - can view and approve/reject leaves
    [HttpGet("pending")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<List<LeaveDto>>>> GetPendingLeaves()
    {
        var query = new GetPendingLeavesQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<List<LeaveDto>>>> GetAllLeaves()
    {
        var query = new GetAllLeavesQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{id}/approve")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<LeaveDto>>> ApproveLeave(Guid id)
    {
        var command = new ApproveLeaveCommand(id, CurrentUserId);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/reject")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<LeaveDto>>> RejectLeave(Guid id, [FromBody] RejectLeaveRequest request)
    {
        var command = new RejectLeaveCommand(id, CurrentUserId, request.RejectionReason);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
