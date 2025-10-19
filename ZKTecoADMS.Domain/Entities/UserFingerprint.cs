using System.ComponentModel.DataAnnotations;

namespace ZKTecoADMS.Domain.Entities;

// ZKTeco.Domain/Entities/FingerprintTemplate.cs
public class FingerprintTemplate : BaseEntity
{
    public Guid UserId { get; set; }
    public int FingerIndex { get; set; }

    [Required]
    public string Template { get; set; } = string.Empty;

    public int? TemplateSize { get; set; }
    public int? Quality { get; set; }
    public int Version { get; set; } = 10;

    // Navigation Properties
    public virtual User User { get; set; } = null!;
}