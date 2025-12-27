namespace ZKTecoADMS.Application.Mappings;
using ZKTecoADMS.Application.DTOs.SalaryProfiles;

public class EmployeeSalaryProfileMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EmployeeSalaryProfile, EmployeeSalaryProfileDto>()
            .Map(dest => dest.EmployeeName, src => src.Employee.FirstName + " " + src.Employee.LastName)
            .Map(dest => dest, src => src);
    }
}