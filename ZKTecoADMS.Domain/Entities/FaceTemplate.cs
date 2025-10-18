using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKTecoADMS.Domain.Entities;
// ZKTeco.Domain/Entities/FaceTemplate.cs
public class FaceTemplate
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public int FaceIndex { get; set; } = 0;

    [Required]
    public string Template { get; set; } = string.Empty;

    public int? TemplateSize { get; set; }
    public byte[]? PhotoData { get; set; }
    public int Version { get; set; } = 50;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual User User { get; set; } = null!;
}