using ZKTecoADMS.Application.DTOs.Payslips;

namespace ZKTecoADMS.Application.Mappings;

public class PayslipMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Payslip, PayslipDto>()
        .Map(dest => dest, src => src)
        .Map(dest => dest.EmployeeName, src => src.EmployeeUser != null ? $"{src.EmployeeUser.FirstName} {src.EmployeeUser.LastName}".Trim() : "")
        .Map(dest => dest.SalaryProfileName, src => src.SalaryProfile != null ? src.SalaryProfile.Name : "")
        .Map(dest => dest.StatusName, src => src.Status.ToString())
        .Map(dest => dest.GeneratedByUserName, src => src.GeneratedByUser != null ? $"{src.GeneratedByUser.FirstName} {src.GeneratedByUser.LastName}".Trim() : "")
        .Map(dest => dest.ApprovedByUserName, src => src.ApprovedByUser != null ? $"{src.ApprovedByUser.FirstName} {src.ApprovedByUser.LastName}".Trim() : "");
        
    }
}