using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Employees.CreateEmployee;

public class CreateEmployeeHandler(
    IDeviceService deviceService, 
    IRepository<Employee> employeeRepository,
    IRepository<DeviceCommand> deviceCmdRepository) : ICommandHandler<CreateEmployeeCommand, AppResponse<List<AppResponse<EmployeeDto>>>>
{
    public async Task<AppResponse<List<AppResponse<EmployeeDto>>>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employees = request.DeviceIds.Select(deviceId => new Employee
            {
                Pin = request.Pin,
                Name = request.Name,
                CardNumber = request.CardNumber,
                Password = request.Password,
                Privilege = request.Privilege,
                Department = request.Department,
                DeviceId = deviceId
            })
            .ToList();
        var employeeResults = new List<AppResponse<EmployeeDto>>();
        
        foreach (var employee in employees)
        {
            var validEmployee = await deviceService.IsValidEmployeeAsync(employee);
            if (!validEmployee.IsSuccess)
            {
                employeeResults.Add(AppResponse<EmployeeDto>.Error(validEmployee.Errors));
                continue;
            }
            
            var employeeEntity = await employeeRepository.AddAsync(employee, cancellationToken);
            employeeResults.Add(AppResponse<EmployeeDto>.Success(employeeEntity.Adapt<EmployeeDto>()));
            
            var cmd = new DeviceCommand
            {
                DeviceId = employeeEntity.DeviceId,
                Command = ClockCommandBuilder.BuildAddOrUpdateEmployeeCommand(employeeEntity),
                Priority = 10,
                Status = CommandStatus.Created,
                CommandType = DeviceCommandTypes.AddEmployee,
                ObjectReferenceId = employeeEntity.Id
            };
            await deviceCmdRepository.AddAsync(cmd, cancellationToken);
        }
        
        return AppResponse<List<AppResponse<EmployeeDto>>>.Success(employeeResults);
    }
}