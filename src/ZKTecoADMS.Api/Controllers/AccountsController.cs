using Mapster;
using Microsoft.AspNetCore.Authorization;
using ZKTecoADMS.Application.Commands.Accounts.CreateEmployeeAccount;
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
}

public class CreateEmployeeAccountRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Guid UserDeviceId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Department { get; set; }
}