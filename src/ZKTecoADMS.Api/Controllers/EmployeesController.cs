using Mapster;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.Employees.CreateEmployee;
using ZKTecoADMS.Application.Commands.Employees.DeleteEmployee;
using ZKTecoADMS.Application.Commands.Employees.UpdateEmployee;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.Employees.GetEmployeeDevices;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PolicyNames.AtLeastManager)]
public class EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger, IMediator bus) : AuthenticatedControllerBase
{
    [HttpPost("devices")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByDevices([FromBody] GetEmployeesByDevicesRequest request)
    {
        var query = request.Adapt<GetEmployeeDevicesQuery>();
        
        return Ok(await bus.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<AppResponse<List<AppResponse<EmployeeDto>>>>> CreateEmployee([FromBody] CreateEmployeeRequest request)
    {
        var command = request.Adapt<CreateEmployeeCommand>();
        var created = await bus.Send(command);

        return Ok(created);
    }

    [HttpPut("{employeeId}")]
    public async Task<IActionResult> UpdateEmployee(Guid employeeId, [FromBody] UpdateEmployeeRequest request)
    {
        var cmd = new UpdateEmployeeCommand(
            employeeId,
            request.PIN,
            request.FullName,
            request.CardNumber,
            request.Password,
            request.Privilege,
            request.Email,
            request.PhoneNumber,
            request.Department,
            request.DeviceId);
        
        return Ok(await bus.Send(cmd));
    }

    [HttpDelete("{employeeId}")]
    public async Task<IActionResult> DeleteEmployee(Guid employeeId)
    {
        var cmd = new DeleteEmployeeCommand(employeeId);

        return Ok(await bus.Send(cmd));
    }

}