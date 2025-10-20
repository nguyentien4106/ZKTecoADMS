using ZKTecoADMS.Api.Models.Requests;
using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.Devices.GetDevicesByUser;
using ZKTecoADMS.Application.Queries.Devices.GetCommandsByDevice;
using ZKTecoADMS.Application.Queries.Devices.GetAllDevices;
using ZKTecoADMS.Application.Queries.Devices.GetDeviceById;
using ZKTecoADMS.Domain.Exceptions;
using ZKTecoADMS.Domain.Entities;
using Mapster;
using ZKTecoADMS.Application.Commands.Devices.AddDevice;

namespace ZKTecoADMS.API.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController(
        IDeviceService deviceService, 
        ILogger<DevicesController> logger, 
        IMediator _bus
        ) : ControllerBase
    {
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<AppResponse<DeviceResponse>>> GetDevicesByUser(Guid userId)
        {
            var query = new GetDevicesByUserQuery(userId);
            return Ok(await _bus.Send(query));
        }

        [HttpGet("{id}/commands")]
        public async Task<ActionResult<AppResponse<IEnumerable<DeviceCmdResponse>>>> GetCommandsByDevice(Guid deviceId)
        {
            var query = new GetCommandsByDeviceQuery(deviceId);
            return Ok(await _bus.Send(query));
        }
        
        [HttpGet]
        public async Task<ActionResult<AppResponse<PagedResult<DeviceResponse>>>> GetAllDevices([FromQuery] PaginationRequest request)
        {
            var query = new GetAllDevicesQuery(request);
            return Ok(await _bus.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppResponse<DeviceResponse>>> GetDeviceById(Guid id)
        {
            var query = new GetDeviceByIdQuery(id);
            return Ok(await _bus.Send(query));
        }

        [HttpPost]
        public async Task<ActionResult<DeviceResponse>> AddDevice([FromBody] AddDeviceRequest request)
        {
            var cmd = request.Adapt<AddDeviceCommand>();
            
            return Ok(await _bus.Send(cmd));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(Guid id)
        {
            await deviceService.DeleteDeviceAsync(id);
            return NoContent();
        }

        [HttpPost("{sn}/commands")]
        public async Task<ActionResult<DeviceCommand>> SendCommand(string sn, [FromBody] SendCommandRequest request)
        {
            var device = await deviceService.GetDeviceBySerialNumberAsync(sn);
            if (device == null)
            {
                return NotFound();
            }

            var command = new DeviceCommand
            {
                DeviceId = device.Id,
                Command = request.Command,
                Priority = request.Priority
            };

            var created = await deviceService.CreateCommandAsync(command);
            return Ok(created);
        }

        [HttpGet("{id}/commands/pending")]
        public async Task<ActionResult<IEnumerable<DeviceCommand>>> GetPendingCommands(Guid id)
        {
            var commands = await deviceService.GetPendingCommandsAsync(id);
            return Ok(commands);
        }
}