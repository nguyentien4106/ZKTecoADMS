using MediatR;

namespace ZKTecoADMS.Application.Commands.Employees.CreateEmployee;

public class CreateEmployeeHandler(
    IRepository<Employee> employeeRepository) : IRequestHandler<CreateEmployeeCommand, AppResponse<Guid>>
{
    public async Task<AppResponse<Guid>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Check if employee code already exists
        var existingEmployee = await employeeRepository.GetSingleAsync(
            e => e.EmployeeCode == request.EmployeeCode || e.CompanyEmail == request.CompanyEmail,
            cancellationToken: cancellationToken
        );

        if(existingEmployee == null)
        {
            var employee = request.Adapt<Employee>();

            await employeeRepository.AddAsync(employee, cancellationToken);

            return AppResponse<Guid>.Success(employee.Id);
        }

        if (existingEmployee.EmployeeCode == request.EmployeeCode)
        {
            return AppResponse<Guid>.Error($"Employee with code {request.EmployeeCode} already exists");
        }

        return AppResponse<Guid>.Error($"Employee with company email {request.CompanyEmail} already exists");
    }
}
