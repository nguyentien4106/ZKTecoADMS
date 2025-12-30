using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZKTecoADMS.Application.Commands.Employees.CreateEmployee;
using ZKTecoADMS.Application.Commands.Employees.UpdateEmployee;
using ZKTecoADMS.Application.Commands.Employees.DeleteEmployee;
using ZKTecoADMS.Application.Queries.Employees.GetEmployees;
using ZKTecoADMS.Application.Queries.Employees.GetEmployeeById;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Employees;
using Mapster;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IMediator mediator) : AuthenticatedControllerBase
{
    [HttpGet]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<List<EmployeeDto>>>> GetEmployees([FromQuery] string? searchTerm, [FromQuery] string? employmentType, [FromQuery] string? workStatus)
    {
        var query = new GetEmployeesQuery
        {
            SearchTerm = searchTerm,
            EmploymentType = employmentType,
            WorkStatus = workStatus,
            ManagerId = CurrentUserId
        };
        
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(Guid id)
    {
        var result = await mediator.Send(new GetEmployeeByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
    {
        var command = request.Adapt<CreateEmployeeCommand>();
        command.ManagerId = CurrentUserId;

        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeCommand command)
    {
        command.Id = id;
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        var result = await mediator.Send(new DeleteEmployeeCommand { Id = id });
        return Ok(result);
    }
}
