using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Mappings;

public class ShiftMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Shift, ShiftDto>()
            .Map(dest => dest.EmployeeName, src => $"{src.EmployeeUser.FirstName} {src.EmployeeUser.LastName}".Trim())
            .Map(dest => dest, src => src);

    }

}