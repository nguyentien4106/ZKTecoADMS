using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Commands.Accounts.CreateEmployeeAccount;

public class CreateEmployeeAccountCommand : ICommand<AppResponse<EmployeeAccountDto>>
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public Guid EmployeeDeviceId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public Guid ManagerId { get; set; }
}