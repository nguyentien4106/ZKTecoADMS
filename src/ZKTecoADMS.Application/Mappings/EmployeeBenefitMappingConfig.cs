using ZKTecoADMS.Application.DTOs.Benefits;

namespace ZKTecoADMS.Application.Mappings;

public class EmployeeBenefitMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EmployeeBenefit, EmployeeBenefitDto>()
            .Map(dest => dest.Employee, src => src.Employee)
            .Map(dest => dest.Benefit, src => src.Benefit)
            .Map(dest => dest, src => src);
    }
}