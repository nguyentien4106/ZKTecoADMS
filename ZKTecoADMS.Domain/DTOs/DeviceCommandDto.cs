using System;

namespace ZKTecoADMS.Domain.DTOs;

public class DeviceCommandDto
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string CommandType { get; set; } = string.Empty;
    public string? CommandData { get; set; }
    public int Priority { get; set; }
    public string Status { get; set; } = "Pending";
    public string? ResponseData { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}