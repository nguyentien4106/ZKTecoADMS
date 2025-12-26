using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Mappings;

public class EmployeeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Employee, EmployeeDto>()
            .Map(dest => dest.HasAccount, src => src.ApplicationUserId != null)
            .Map(dest => dest, src => src);
    }
}