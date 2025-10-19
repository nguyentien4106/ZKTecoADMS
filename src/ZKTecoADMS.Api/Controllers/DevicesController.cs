using ZKTecoADMS.Api.Models.Requests;
using ZKTecoADMS.Api.Models.Responses;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Core.Services;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.API.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger)
        : ControllerBase
    {
        [HttpGet("{id}/commands")]
        public async Task<ActionResult<IEnumerable<DeviceCommand>>> GetAllDeviceCommands(Guid deviceId)
        {
            var commands = await deviceService.GetAllDeviceCommandsAsync(deviceId);
            return Ok(commands);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetAllDevices()
        {
            var devices = await deviceService.GetAllDevicesAsync();
            var response = devices.Select(d => new DeviceResponse
            {
                Id = d.Id,
                SerialNumber = d.SerialNumber,
                DeviceName = d.DeviceName,
                Model = d.Model,
                IpAddress = d.IpAddress,
                DeviceStatus = d.DeviceStatus,
                LastOnline = d.LastOnline,
                IsActive = d.IsActive
            });
            
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceResponse>> GetDevice(Guid id)
        {
            var device = await deviceService.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            var response = new DeviceResponse
            {
                Id = device.Id,
                SerialNumber = device.SerialNumber,
                DeviceName = device.DeviceName,
                Model = device.Model,
                IpAddress = device.IpAddress,
                DeviceStatus = device.DeviceStatus,
                LastOnline = device.LastOnline,
                IsActive = device.IsActive
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<DeviceResponse>> RegisterDevice([FromBody] DeviceRegisterRequest request)
        {
            try
            {
                var device = new Device
                {
                    SerialNumber = request.SerialNumber,
                    DeviceName = request.DeviceName,
                    Model = request.Model,
                    IpAddress = request.IpAddress,
                    Port = request.Port,
                    Location = request.Location,
                    IsActive = false
                };

                var created = await deviceService.RegisterDeviceAsync(device);
                var response = new DeviceResponse
                {
                    Id = created.Id,
                    SerialNumber = created.SerialNumber,
                    DeviceName = created.DeviceName,
                    Model = created.Model,
                    IpAddress = created.IpAddress,
                    DeviceStatus = created.DeviceStatus,
                    IsActive = created.IsActive
                };

                return CreatedAtAction(nameof(GetDevice), new { id = created.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
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