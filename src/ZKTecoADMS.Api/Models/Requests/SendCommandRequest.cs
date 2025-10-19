namespace ZKTecoADMS.Api.Models.Requests;

public class SendCommandRequest
{
    public string Command { get; set; } = string.Empty;
    public int Priority { get; set; } = 1;
}