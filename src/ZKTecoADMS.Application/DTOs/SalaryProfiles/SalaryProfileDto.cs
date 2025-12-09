using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.DTOs.SalaryProfiles;

public class SalaryProfileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SalaryRateType RateType { get; set; }
    public string RateTypeName { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal? OvertimeMultiplier { get; set; }
    public decimal? HolidayMultiplier { get; set; }
    public decimal? NightShiftMultiplier { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
