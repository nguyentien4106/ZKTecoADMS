using Mapster;
using ZKTecoADMS.Application.Commands.Users.CreateUser;
using ZKTecoADMS.Application.Commands.Users.DeleteUser;
using ZKTecoADMS.Application.Commands.Users.UpdateUser;
using ZKTecoADMS.Application.DTOs.Users;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.Users.GetUserDevices;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, ILogger<UsersController> logger, IMediator bus) : ControllerBase
{
    [HttpPost("devices")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersByDevices([FromBody] GetUsersByDevicesRequest request)
    {
        var query = request.Adapt<GetUserDevicesQuery>();
        
        return Ok(await bus.Send(query));
    }

    [HttpPost]
    public async Task<ActionResult<List<AppResponse<UserDto>>>> CreateUser([FromBody] CreateUserRequest request)
    {
        var command = request.Adapt<CreateUserCommand>();
        var created = await bus.Send(command);

        return Ok(created);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserRequest request)
    {
        var cmd = new UpdateUserCommand(
            userId,
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

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var cmd = new DeleteUserCommand(userId);

        return Ok(await bus.Send(cmd));
    }

}