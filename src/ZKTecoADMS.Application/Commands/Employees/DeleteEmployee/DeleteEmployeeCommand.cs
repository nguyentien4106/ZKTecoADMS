using MediatR;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Application.Commands.Employees.DeleteEmployee;

public class DeleteEmployeeCommand : IRequest<AppResponse<bool>>
{
    public Guid Id { get; set; }
}
