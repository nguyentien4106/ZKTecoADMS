using MediatR;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Employees.DeleteEmployee;

public class DeleteEmployeeHandler(IRepository<Employee> employeeRepository) 
    : IRequestHandler<DeleteEmployeeCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.Id);
        
        if (employee == null)
        {
            return AppResponse<bool>.Error("Employee not found");
        }

        await employeeRepository.DeleteAsync(employee, cancellationToken);

        return AppResponse<bool>.Success(true);
    }
}
