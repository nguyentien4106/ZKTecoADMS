using ZKTecoADMS.Application.DTOs.Attendances;

namespace ZKTecoADMS.Application.Commands.Attendances.GetAttendancesByDevices;

public record GetAttsByDevicesCommand(PaginationRequest PaginationRequest, GetAttendancesByDeviceRequest Filter) : ICommand<AppResponse<PagedResult<AttendanceDto>>>;