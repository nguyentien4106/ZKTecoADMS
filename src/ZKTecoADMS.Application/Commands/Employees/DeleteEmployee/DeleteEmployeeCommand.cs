namespace ZKTecoADMS.Application.Commands.Employees.DeleteEmployee;

public record DeleteEmployeeCommand(Guid EmployeeId) : ICommand<AppResponse<Guid>>;