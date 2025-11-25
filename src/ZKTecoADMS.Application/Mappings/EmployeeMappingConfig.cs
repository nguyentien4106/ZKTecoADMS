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
            .Map(dest => dest, src => src)
            .Map(dest => dest.ApplicationUser, src => src.ApplicationUser != null
                ? new AccountDto
                {
                    Email = src.ApplicationUser.Email!,
                    FirstName = src.ApplicationUser.FirstName!,
                    LastName = src.ApplicationUser.LastName!,
                    PhoneNumber = src.ApplicationUser.PhoneNumber,
                    UserName = src.ApplicationUser.UserName,
                    Id = src.ApplicationUser.Id
                }
                : null);
       
    }
}