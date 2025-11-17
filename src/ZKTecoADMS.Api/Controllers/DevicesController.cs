using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.Devices.GetDevicesByUser;
using ZKTecoADMS.Application.Queries.Devices.GetAllDevices;
using ZKTecoADMS.Application.Queries.Devices.GetDeviceById;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.Devices.ToggleActive;
using ZKTecoADMS.Application.Commands.Devices.AddDevice;
using ZKTecoADMS.Application.Commands.Devices.DeleteDevice;
using ZKTecoADMS.Application.Queries.Devices.GetDeviceInfo;
using ZKTecoADMS.Application.Constants;

namespace ZKTecoADMS.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DevicesController(
    IMediator bus
    ) : AuthenticatedControllerBase
{
    [HttpGet("users/{userId}")]
    public async Task<ActionResult<AppResponse<DeviceDto>>> GetDevicesByUser(Guid userId)
    {
        var query = new GetDevicesByUserQuery(userId);
        return Ok(await bus.Send(query));
    }
    
    [HttpGet]
    public async Task<ActionResult<AppResponse<IEnumerable<DeviceDto>>>> GetAllDevices()
    {
        var query = new GetAllDevicesQuery(
            UserId: CurrentUserId,
            IsAdminRequest: IsAdmin
        );
        
        return Ok(await bus.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppResponse<DeviceDto>>> GetDeviceById(Guid id)
    {
        var query = new GetDeviceByIdQuery(id);
        return Ok(await bus.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<DeviceDto>> AddDevice([FromBody] AddDeviceRequest request)
    {
        var cmd = request.Adapt<AddDeviceCommand>();
        cmd.ApplicationUserId = CurrentUserId;
        
        return Ok(await bus.Send(cmd));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(Guid id)
    {
        var cmd = new DeleteDeviceCommand(id);
        return Ok(await bus.Send(cmd));
    }

    [Authorize(Policy = PolicyNames.AdminOnly)]
    [HttpPut("{id}/toggle-active")]
    public async Task<ActionResult<AppResponse<DeviceDto>>> ActiveDevice(Guid id)
    {
        var cmd = new ToggleActiveCommand(id);
        return Ok(await bus.Send(cmd));
    }
    
    [HttpGet("{deviceId}/device-info")]
    public async Task<ActionResult<AppResponse<DeviceInfoDto>>> GetDeviceInfo(Guid deviceId)
    {
        var query = new GetDeviceInfoQuery(deviceId);
        return Ok(await bus.Send(query));
    }
    
}