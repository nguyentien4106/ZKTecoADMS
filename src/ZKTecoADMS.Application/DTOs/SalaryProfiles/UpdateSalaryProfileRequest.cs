using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.DTOs.SalaryProfiles;

public class UpdateSalaryProfileRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SalaryRateType RateType { get; set; }
    public decimal Rate { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal? OvertimeMultiplier { get; set; }
    public decimal? HolidayMultiplier { get; set; }
    public decimal? NightShiftMultiplier { get; set; }
    public bool IsActive { get; set; }
}
