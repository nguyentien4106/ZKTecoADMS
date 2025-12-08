using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Mappings;

public class ShiftMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Shift, ShiftDto>()
            .Map(dest => dest.EmployeeName, src => src.EmployeeUser != null ? $"{src.EmployeeUser.FirstName} {src.EmployeeUser.LastName}".Trim() : null )
            .Map(dest => dest.CheckInTime, src => src.CheckInAttendance != null ? src.CheckInAttendance.AttendanceTime : (DateTime?)null)
            .Map(dest => dest.CheckOutTime, src => src.CheckOutAttendance != null ? src.CheckOutAttendance.AttendanceTime : (DateTime?)null)
            .Map(dest => dest, src => src);

    }

}