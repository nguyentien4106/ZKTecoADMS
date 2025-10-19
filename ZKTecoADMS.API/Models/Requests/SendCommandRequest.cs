namespace ZKTecoADMS.API.Models.Requests;

public class SendCommandRequest
{
    public string CommandType { get; set; } = string.Empty;
    public string? CommandData { get; set; }
    public int Priority { get; set; } = 1;
}