using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Employees.UpdateEmployee;

public class UpdateEmployeeHandler(
    IRepository<Employee> employeeRepository,
    IRepository<DeviceCommand> deviceCmdRepository
    ) : ICommandHandler<UpdateEmployeeCommand, AppResponse<EmployeeDto>>
{
    public async Task<AppResponse<EmployeeDto>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken: cancellationToken);
        if (employee == null)
        {
            return  AppResponse<EmployeeDto>.Fail("Employee not found");
        }
        employee.Pin = request.PIN;
        employee.Name = request.FullName;
        employee.CardNumber = request.CardNumber;
        employee.Password = request.Password;
        employee.Privilege = request.Privilege;
        employee.Department = request.Department;
        employee.IsActive = false;
        
        await employeeRepository.UpdateAsync(employee, cancellationToken);
        
        var cmd = new DeviceCommand
        {
            DeviceId = employee.DeviceId,
            Command = ClockCommandBuilder.BuildAddOrUpdateEmployeeCommand(employee),
            Priority = 10,
            Status = CommandStatus.Created,
            CommandType = DeviceCommandTypes.UpdateEmployee,
            ObjectReferenceId = employee.Id,
        };
        await deviceCmdRepository.AddAsync(cmd, cancellationToken);
        
        return AppResponse<EmployeeDto>.Success(employee.Adapt<EmployeeDto>());
    }
}