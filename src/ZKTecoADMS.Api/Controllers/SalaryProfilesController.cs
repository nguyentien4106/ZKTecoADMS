using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.SalaryProfiles.CreateSalaryProfile;
using ZKTecoADMS.Application.Commands.SalaryProfiles.UpdateSalaryProfile;
using ZKTecoADMS.Application.Commands.SalaryProfiles.DeleteSalaryProfile;
using ZKTecoADMS.Application.Commands.SalaryProfiles.AssignSalaryProfile;
using ZKTecoADMS.Application.Queries.SalaryProfiles.GetAllSalaryProfiles;
using ZKTecoADMS.Application.Queries.SalaryProfiles.GetSalaryProfileById;
using ZKTecoADMS.Application.Queries.SalaryProfiles.GetEmployeeSalaryProfile;
using ZKTecoADMS.Api.Models.Responses;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Application.Models;
using Mapster;

namespace ZKTecoADMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalaryProfilesController(IMediator mediator) : AuthenticatedControllerBase
{
    /// <summary>
    /// Get all salary profiles
    /// </summary>
    [HttpGet]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<List<SalaryProfileDto>>>> GetAllProfiles([FromQuery] bool? activeOnly = null)
    {
        var query = new GetAllSalaryProfilesQuery(activeOnly);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get salary profile by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<SalaryProfileDto>>> GetProfileById(Guid id)
    {
        var query = new GetSalaryProfileByIdQuery(id);
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new salary profile
    /// </summary>
    [HttpPost]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<SalaryProfileDto>>> CreateProfile([FromBody] CreateSalaryProfileRequest request)
    {
        var command = request.Adapt<CreateSalaryProfileCommand>();
        
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Update an existing salary profile
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<SalaryProfileDto>>> UpdateProfile(Guid id, [FromBody] UpdateSalaryProfileRequest request)
    {
        var command = request.Adapt<UpdateSalaryProfileCommand>();
        command = command with { Id = id };
        
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Delete a salary profile
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<bool>>> DeleteProfile(Guid id)
    {
        var command = new DeleteSalaryProfileCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Assign a salary profile to an employee
    /// </summary>
    [HttpPost("assign")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<EmployeeSalaryProfileDto>>> AssignProfile([FromBody] AssignSalaryProfileRequest request)
    {
        var command = new AssignSalaryProfileCommand(
            request.EmployeeId,
            request.SalaryProfileId,
            request.EffectiveDate,
            request.Notes
        );
        
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Get active salary profile for an employee
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<AppResponse<EmployeeSalaryProfileDto>>> GetEmployeeSalaryProfile(Guid employeeId)
    {
        var query = new GetEmployeeSalaryProfileQuery(employeeId);
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
