using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.DTOs.Attendances;

public record AttendanceDto(
    Guid Id,
    DateTime AttendanceTime,
    string DeviceName,
    string UserName,
    VerifyModes VerifyMode,
    AttendanceStates AttendanceState
);