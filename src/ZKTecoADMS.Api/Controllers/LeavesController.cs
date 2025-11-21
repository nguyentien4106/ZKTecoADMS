using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.Leaves.CreateLeave;
using ZKTecoADMS.Application.Commands.Leaves.CancelLeave;
using ZKTecoADMS.Application.Commands.Leaves.ApproveLeave;
using ZKTecoADMS.Application.Commands.Leaves.RejectLeave;
using ZKTecoADMS.Application.Queries.Leaves.GetMyLeaves;
using ZKTecoADMS.Application.Queries.Leaves.GetPendingLeaves;
using ZKTecoADMS.Application.Queries.Leaves.GetAllLeaves;
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
        var query = new GetMyLeavesQuery(CurrentUserId, IsManager);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<LeaveDto>>> CreateLeave([FromBody] CreateLeaveRequest request)
    {
        if(ManagerId.HasValue == false)
        {
            return AppResponse<LeaveDto>.Error("ManagerId is not assigned to the current user.");
        }

        var managerId = request.EmployeeUserId.HasValue ? CurrentUserId : ManagerId.Value;
        var employeeUserId = request.EmployeeUserId ?? CurrentUserId;

        var command = new CreateLeaveCommand(
            employeeUserId,
            managerId,
            request.ShiftId,
            request.StartDate,
            request.EndDate,
            request.Type,
            request.IsHalfShift,
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
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<List<LeaveDto>>>> GetPendingLeaves()
    {
        var query = new GetPendingLeavesQuery(CurrentUserId, IsManager);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<List<LeaveDto>>>> GetAllLeaves()
    {
        var query = new GetAllLeavesQuery(CurrentUserId, IsManager);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{id}/approve")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<bool>>> ApproveLeave(Guid id)
    {
        var command = new ApproveLeaveCommand(id, CurrentUserId);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/reject")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<bool>>> RejectLeave(Guid id, [FromBody] RejectLeaveRequest request)
    {
        var command = new RejectLeaveCommand(id, CurrentUserId, request.RejectionReason);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
