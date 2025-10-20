using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.Devices.GetDevicesByUser;
using ZKTecoADMS.Application.Queries.Devices.GetCommandsByDevice;
using ZKTecoADMS.Application.Queries.Devices.GetAllDevices;
using ZKTecoADMS.Application.Queries.Devices.GetDeviceById;
using ZKTecoADMS.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Application.Commands.Devices.ActiveDeviceCommand;
using ZKTecoADMS.Application.Commands.Devices.AddDevice;
using ZKTecoADMS.Application.Commands.Devices.CreateDeviceCmd;
using ZKTecoADMS.Application.Commands.Devices.DeleteDevice;
using ZKTecoADMS.Application.Queries.Devices.GetPendingCommands;

namespace ZKTecoADMS.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DevicesController(
    IMediator bus
    ) : ControllerBase
{
    [HttpGet("users/{userId}")]
    public async Task<ActionResult<AppResponse<DeviceResponse>>> GetDevicesByUser(Guid userId)
    {
        var query = new GetDevicesByUserQuery(userId);
        return Ok(await bus.Send(query));
    }

    [HttpGet("{id}/commands")]
    public async Task<ActionResult<AppResponse<IEnumerable<DeviceCmdResponse>>>> GetCommandsByDevice(Guid deviceId)
    {
        var query = new GetCommandsByDeviceQuery(deviceId);
        return Ok(await bus.Send(query));
    }
    
    [HttpGet]
    public async Task<ActionResult<AppResponse<PagedResult<DeviceResponse>>>> GetAllDevices([FromQuery] PaginationRequest request)
    {
        var query = new GetAllDevicesQuery(request);
        return Ok(await bus.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppResponse<DeviceResponse>>> GetDeviceById(Guid id)
    {
        var query = new GetDeviceByIdQuery(id);
        return Ok(await bus.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<DeviceResponse>> AddDevice([FromBody] AddDeviceRequest request)
    {
        var cmd = request.Adapt<AddDeviceCommand>();
        
        return Ok(await bus.Send(cmd));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(Guid id)
    {
        var cmd = new DeleteDeviceCommand(id);
        return Ok(await bus.Send(cmd));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/toggle-active")]
    public async Task<ActionResult<AppResponse<bool>>> ActiveDevice(Guid id)
    {
        var cmd = new ToggleActiveCommand(id);
        return Ok(await bus.Send(cmd));
    }

    [HttpPost("{deviceId}/commands")]
    public async Task<ActionResult<DeviceCmdResponse>> CreateDeviceCommand(Guid deviceId, [FromBody] DeviceCmdRequest request)
    {
        var cmd = new CreateDeviceCmdCommand(deviceId, request.Command, request.Priority);
        
        return Ok(await bus.Send(cmd));
    }

    [HttpGet("{deviceId}/commands/pending")]
    public async Task<ActionResult<AppResponse<IEnumerable<DeviceCommand>>>> GetPendingCommands(Guid deviceId)
    {
        var query = new GetPendingCmdQuery(deviceId);
        
        return Ok(await bus.Send(query));
    }
}