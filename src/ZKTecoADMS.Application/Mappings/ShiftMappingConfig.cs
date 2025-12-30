using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Mappings;

public class ShiftMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Shift, ShiftDto>()
            .Map(dest => dest.EmployeeCode, src => src.Employee.EmployeeCode)
            .Map(dest => dest.EmployeeName, src => src.Employee.GetFullName())
            .Map(dest => dest.StartTime, src => TimeOnly.FromDateTime(src.StartTime))
            .Map(dest => dest.EndTime, src => TimeOnly.FromDateTime(src.EndTime))
            .Map(dest => dest.Date, src => src.StartTime.Date)
            .Map(dest => dest, src => src);

    }

}