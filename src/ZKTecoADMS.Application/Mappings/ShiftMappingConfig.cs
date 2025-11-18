using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Mappings;

public class ShiftMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Shift, ShiftDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ApplicationUserId, src => src.ApplicationUserId)
            .Map(dest => dest.EmployeeName, src => src.ApplicationUser != null 
                ? $"{src.ApplicationUser.FirstName} {src.ApplicationUser.LastName}".Trim() 
                : string.Empty)
            .Map(dest => dest.StartTime, src => src.StartTime)
            .Map(dest => dest.EndTime, src => src.EndTime)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.ApprovedByUserId, src => src.ApprovedByUserId)
            .Map(dest => dest.ApprovedByUserName, src => src.ApprovedByUser != null 
                ? $"{src.ApprovedByUser.FirstName} {src.ApprovedByUser.LastName}".Trim() 
                : null)
            .Map(dest => dest.ApprovedAt, src => src.ApprovedAt)
            .Map(dest => dest.RejectionReason, src => src.RejectionReason)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .Map(dest => dest.TotalHours, src => CalculateTotalHours(src.StartTime, src.EndTime));
    }

    private static double CalculateTotalHours(DateTime startTime, DateTime endTime)
    {
        var duration = endTime - startTime;
        return Math.Round(duration.TotalHours, 2);
    }
}