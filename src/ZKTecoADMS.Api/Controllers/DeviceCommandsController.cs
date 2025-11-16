using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.DeviceCommands.GetCommandsByDevice;
using ZKTecoADMS.Application.Commands.DeviceCommands.CreateDeviceCmd;
using ZKTecoADMS.Application.Queries.DeviceCommands.GetPendingCommands;
using ZKTecoADMS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Api.Controllers.Base;

namespace ZKTecoADMS.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/devices/{deviceId}/commands")]
public class DeviceCommandsController(
    IMediator bus
    ) : AuthenticatedControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AppResponse<IEnumerable<DeviceCmdDto>>>> GetCommandsByDevice(Guid deviceId)
    {
        var query = new GetCommandsByDeviceQuery(deviceId);
        return Ok(await bus.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<DeviceCmdDto>> CreateDeviceCommand(Guid deviceId, [FromBody] DeviceCmdRequest request)
    {
        var cmd = new CreateDeviceCmdCommand(deviceId, request.CommandType, request.Priority);
        
        return Ok(await bus.Send(cmd));
    }

    [HttpGet("pending")]
    public async Task<ActionResult<AppResponse<IEnumerable<DeviceCommand>>>> GetPendingCommands(Guid deviceId)
    {
        var query = new GetPendingCmdQuery(deviceId);

        return Ok(await bus.Send(query));
    }
}