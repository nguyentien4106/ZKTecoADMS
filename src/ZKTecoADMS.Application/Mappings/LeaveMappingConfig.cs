using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Mappings;

public class LeaveMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Leave, LeaveDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.EmployeeUserId, src => src.EmployeeUserId)
            .Map(dest => dest.EmployeeName, src => $"{src.ApplicationUser.FirstName} {src.ApplicationUser.LastName}".Trim())
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Shift, src => src.Shift.Adapt<ShiftDto>())
            .Map(dest => dest.IsHalfShift, src => src.IsHalfShift)
            .Map(dest => dest.Reason, src => src.Reason)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.RejectionReason, src => src.RejectionReason)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
    }
}
