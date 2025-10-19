using Microsoft.AspNetCore.Mvc;
using ZKTecoADMS.API.Models.Requests;
using ZKTecoADMS.Core.Services;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("pin/{pin}")]
    public async Task<ActionResult<User>> GetUserByPIN(string pin)
    {
        var user = await _userService.GetUserByPINAsync(pin);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = new User
            {
                PIN = request.PIN,
                FullName = request.FullName,
                CardNumber = request.CardNumber,
                Password = request.Password,
                GroupId = request.GroupId,
                Privilege = request.Privilege,
                VerifyMode = request.VerifyMode,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Department = request.Department,
                Position = request.Position,
                IsActive = true
            };

            var created = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        var user = await _userService.GetUserByIdAsync(id);
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

        await _userService.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/sync-to-device/{deviceId}")]
    public async Task<IActionResult> SyncUserToDevice(Guid id, Guid deviceId)
    {
        try
        {
            await _userService.SyncUserToDeviceAsync(id, deviceId);
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
            await _userService.SyncUserToAllDevicesAsync(id);
            return Ok(new { message = "User sync to all devices initiated" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/device-mappings")]
    public async Task<ActionResult<IEnumerable<UserDeviceMapping>>> GetUserDeviceMappings(Guid id)
    {
        var mappings = await _userService.GetUserDeviceMappingsAsync(id);
        return Ok(mappings);
    }
}