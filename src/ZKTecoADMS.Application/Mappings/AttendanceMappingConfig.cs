using ZKTecoADMS.Application.DTOs.Attendances;

namespace ZKTecoADMS.Application.Mappings;

public class AttendanceMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Attendance, AttendanceDto>()
            .Map(dest => dest.DeviceName, src => src.Device.DeviceName)
            .Map(dest => dest.UserName, src => src.Employee.Name)
            .Map(dest => dest, src => src);
    }
}