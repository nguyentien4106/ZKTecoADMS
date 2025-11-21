using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Mappings;

public class LeaveMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Leave, LeaveDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ApplicationUserId, src => src.ApplicationUserId)
            .Map(dest => dest.EmployeeName, src => $"{src.ApplicationUser.FirstName} {src.ApplicationUser.LastName}".Trim())
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.StartDate, src => src.StartDate)
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.IsFullDay, src => src.IsFullDay)
            .Map(dest => dest.Reason, src => src.Reason)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.ApprovedByUserId, src => src.ApprovedByUserId)
            .Map(dest => dest.ApprovedByUserName, src => src.ApprovedByUser != null 
                ? $"{src.ApprovedByUser.FirstName} {src.ApprovedByUser.LastName}".Trim() 
                : null)
            .Map(dest => dest.ApprovedAt, src => src.ApprovedAt)
            .Map(dest => dest.RejectionReason, src => src.RejectionReason)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
    }
}
