using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Mappings;

public class EmployeeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Employee, EmployeeDto>()
            .Map(dest => dest.HasAccount, src => src.ApplicationUserId != null)
            .Map(dest => dest.HasDeviceUsers, src => src.DeviceUsers != null && src.DeviceUsers.Any())
            .Map(dest => dest.HasBenefits, src => src.Benefits != null && src.Benefits.Any())
            .Map(dest => dest, src => src);
    }
}