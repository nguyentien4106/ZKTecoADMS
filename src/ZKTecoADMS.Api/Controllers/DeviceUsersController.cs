using Mapster;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.DeviceUsers.Create;
using ZKTecoADMS.Application.Commands.DeviceUsers.Delete;
using ZKTecoADMS.Application.Commands.DeviceUsers.MapEmployee;
using ZKTecoADMS.Application.Commands.DeviceUsers.Update;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.DeviceUsers;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.DeviceUsers.GetDeviceUserDevices;

namespace ZKTecoADMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = PolicyNames.AtLeastManager)]
public class DeviceUsersController(IMediator bus) : AuthenticatedControllerBase
{
    [HttpPost("devices")]
    [Authorize(Policy = PolicyNames.AtLeastEmployee)]
    public async Task<ActionResult<IEnumerable<DeviceUserDto>>> GetDeviceUsersByDevices([FromBody] GetDeviceUsersByDevicesRequest request)
    {
        var query = request.Adapt<GetDeviceUserDevicesQuery>();
        
        return Ok(await bus.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<AppResponse<DeviceUserDto>>> CreateDeviceUser([FromBody] CreateDeviceUserRequest request)
    {
        var command = request.Adapt<CreateDeviceUserCommand>();
        var created = await bus.Send(command);

        return Ok(created);
    }

    [HttpPut("{deviceUserId}")]
    public async Task<IActionResult> UpdateDeviceUser(Guid deviceUserId, [FromBody] UpdateDeviceUserRequest request)
    {
        var cmd = new UpdateDeviceUserCommand(
            deviceUserId,
            request.PIN,
            request.Name,
            request.CardNumber,
            request.Password,
            request.Privilege,
            request.Email,
            request.PhoneNumber,
            request.Department,
            request.DeviceId);
        
        return Ok(await bus.Send(cmd));
    }

    [HttpDelete("{deviceUserId}")]
    public async Task<IActionResult> DeleteDeviceUser(Guid deviceUserId)
    {
        var cmd = new DeleteDeviceUserCommand(deviceUserId);

        return Ok(await bus.Send(cmd));
    }

    [HttpPost("{deviceUserId}/map-employee/{employeeId}")]
    public async Task<ActionResult<AppResponse<EmployeeDto>>> MapDeviceUserToEmployee(Guid deviceUserId, Guid employeeId)
    {
        var cmd = new MapDeviceUserToEmployeeCommand(deviceUserId, employeeId);
        return Ok(await bus.Send(cmd));
    }
}