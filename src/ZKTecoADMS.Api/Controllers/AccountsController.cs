using Mapster;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Application.Commands.Accounts.CreateEmployeeAccount;
using ZKTecoADMS.Application.Commands.Accounts.UpdateEmployeeAccount;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountsController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<AppResponse<bool>> CreateEmployeeAccount([FromBody] CreateEmployeeAccountRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateEmployeeAccountCommand>();
        var result = await mediator.Send(command, cancellationToken);

        return result;
    }

    [HttpPut("{userDeviceId}")]
    [Authorize(Policy = PolicyNames.AtLeastManager)]
    public async Task<AppResponse<bool>> UpdateEmployeeAccount(Guid userDeviceId, [FromBody] UpdateEmployeeAccountRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateEmployeeAccountCommand>();
        command.UserDeviceId = userDeviceId;
        var result = await mediator.Send(command, cancellationToken);

        return result;
    }
}

public class CreateEmployeeAccountRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Guid UserDeviceId { get; set; }
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
}
