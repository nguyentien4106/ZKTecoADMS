using Mapster;
using Microsoft.AspNetCore.Mvc;
using ZKTecoADMS.Api.Models.Requests;
using ZKTecoADMS.Application.Commands.Users.CreateUser;
using ZKTecoADMS.Application.DTOs.Users;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Queries.Users.GetUserDevices;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, ILogger<UsersController> logger, IMediator bus) : ControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;

    [HttpPost("devices")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersByDevices([FromBody] GetUsersByDevicesRequest request)
    {
        var query = request.Adapt<GetUserDevicesQuery>();
        
        return Ok(await bus.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("pin/{pin}")]
    public async Task<ActionResult<User>> GetUserByPIN(string pin)
    {
        var user = await userService.GetUserByPINAsync(pin);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<List<User>>> CreateUser([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(
            request.PIN, 
            request.FullName,
            request.CardNumber, 
            request.Password,
            request.GroupId,
            request.Privilege, 
            request.VerifyMode, 
            request.Email, 
            request.PhoneNumber,
            request.Department, 
            true, 
            request.DeviceIds);
        var created = await bus.Send(command);

        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.FullName = request.FullName ?? user.FullName;
        user.CardNumber = request.CardNumber ?? user.CardNumber;
        user.Email = request.Email ?? user.Email;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        user.Department = request.Department ?? user.Department;
        user.Position = request.Position ?? user.Position;
        user.IsActive = request.IsActive ?? user.IsActive;

        await userService.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/sync-to-device/{deviceId}")]
    public async Task<IActionResult> SyncUserToDevice(Guid id, Guid deviceId)
    {
        try
        {
            await userService.SyncUserToDeviceAsync(id, deviceId);
            return Ok(new { message = "User sync initiated" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/sync-to-all-devices")]
    public async Task<IActionResult> SyncUserToAllDevices(Guid id)
    {
        try
        {
            await userService.SyncUserToAllDevicesAsync(id);
            return Ok(new { message = "User sync to all devices initiated" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}