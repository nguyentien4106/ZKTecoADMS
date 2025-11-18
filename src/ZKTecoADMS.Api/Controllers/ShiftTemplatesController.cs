using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.ShiftTemplates.CreateShiftTemplate;
using ZKTecoADMS.Application.Commands.ShiftTemplates.UpdateShiftTemplate;
using ZKTecoADMS.Application.Commands.ShiftTemplates.DeleteShiftTemplate;
using ZKTecoADMS.Application.Queries.ShiftTemplates.GetShiftTemplates;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftTemplatesController(IMediator mediator) : AuthenticatedControllerBase
{
    [HttpPost]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<ShiftTemplateDto>>> CreateShiftTemplate([FromBody] CreateShiftTemplateRequest request)
    {
        var command = new CreateShiftTemplateCommand(
            CurrentUserId,
            request.Name,
            request.StartTime,
            request.EndTime);   
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<List<ShiftTemplateDto>>>> GetShiftTemplates()
    {
        var query = new GetShiftTemplatesQuery(CurrentUserId);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<ShiftTemplateDto>>> UpdateShiftTemplate(Guid id, [FromBody] UpdateShiftTemplateRequest request)
    {
        var command = new UpdateShiftTemplateCommand(
            id,
            request.Name,
            request.StartTime,
            request.EndTime,
            request.IsActive);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<bool>>> DeleteShiftTemplate(Guid id)
    {
        var command = new DeleteShiftTemplateCommand(id);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
