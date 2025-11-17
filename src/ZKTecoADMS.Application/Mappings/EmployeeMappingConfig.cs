using ZKTecoADMS.Application.Commands.Employees.CreateEmployee;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Mappings;

public class EmployeeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Employee, EmployeeDto>()
            .Map(dest => dest.DeviceName, src => src.Device.DeviceName)
            .Map(dest => dest.Pin, src => src.Pin)
            .Map(dest => dest, src => src)
            .Map(dest => dest.ApplicationUser, src => src.ApplicationUser != null
                ? new EmployeeAccountDto
                {
                    Email = src.ApplicationUser.Email!,
                    FirstName = src.ApplicationUser.FirstName!,
                    LastName = src.ApplicationUser.LastName!,
                    PhoneNumber = src.ApplicationUser.PhoneNumber,
                }
                : null);

        // Explicit mapping for CreateEmployeeRequest to CreateEmployeeCommand
        config.NewConfig<CreateEmployeeRequest, CreateEmployeeCommand>()
            .Map(dest => dest.Pin, src => src.Pin)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.CardNumber, src => src.CardNumber)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.Privilege, src => src.Privilege)
            .Map(dest => dest.Department, src => src.Department)
            .Map(dest => dest.DeviceIds, src => src.DeviceIds);
    }
}