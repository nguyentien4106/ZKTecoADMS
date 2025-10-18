using System.ComponentModel.DataAnnotations;

namespace ZKTecoADMS.Domain.Entities;

// ZKTeco.Domain/Entities/FingerprintTemplate.cs
public class FingerprintTemplate
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public int FingerIndex { get; set; }

    [Required]
    public string Template { get; set; } = string.Empty;

    public int? TemplateSize { get; set; }
    public int? Quality { get; set; }
    public int Version { get; set; } = 10;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual User User { get; set; } = null!;
}