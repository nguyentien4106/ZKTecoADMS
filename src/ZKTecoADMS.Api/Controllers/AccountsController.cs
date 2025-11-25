using Mapster;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Api.Controllers.Base;
using ZKTecoADMS.Application.Commands.Accounts.CreateEmployeeAccount;
using ZKTecoADMS.Application.Commands.Accounts.UpdateEmployeeAccount;
using ZKTecoADMS.Application.Commands.Accounts.UpdateUserProfile;
using ZKTecoADMS.Application.Commands.Accounts.UpdateUserPassword;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Application.DTOs.Users;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Application.Queries.Employees.GetEmployeesByManager;
using ZKTecoADMS.Application.Queries.Users.GetCurrentUserProfile;

namespace ZKTecoADMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IMediator mediator) : AuthenticatedControllerBase
{

    [HttpGet("employees")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<ActionResult<AppResponse<IEnumerable<AccountDto>>>> GetEmployeesByManager(CancellationToken cancellationToken)
    {
        var query = new GetEmployeesByManagerQuery(CurrentUserId);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<AppResponse<AccountDto>> CreateEmployeeAccount([FromBody] CreateEmployeeAccountRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateEmployeeAccountCommand>();
        command.ManagerId = CurrentUserId;
        
        return await mediator.Send(command, cancellationToken);
    }

    [HttpPut("{employeeDeviceId}")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<AppResponse<bool>> UpdateEmployeeAccount(Guid employeeDeviceId, [FromBody] UpdateEmployeeAccountRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateEmployeeAccountCommand>();
        command.EmployeeDeviceId = employeeDeviceId;
        var result = await mediator.Send(command, cancellationToken);

        return result;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<AppResponse<UserProfileDto>>> GetProfile(CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserProfileQuery(CurrentUserId);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<ActionResult<AppResponse<UserProfileDto>>> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserProfileCommand
        {
            UserId = CurrentUserId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("profile/password")]
    public async Task<ActionResult<AppResponse<UserProfileDto>>> UpdatePassword([FromBody] UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserPasswordCommand
        {
            UserId = CurrentUserId,
            CurrentPassword = request.CurrentPassword,
            NewPassword = request.NewPassword
        };
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}

public class CreateEmployeeAccountRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Guid EmployeeDeviceId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
}

public class UpdateEmployeeAccountRequest
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? UserName { get; set; }
}

public class UpdateProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
}

public class UpdatePasswordRequest
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}
