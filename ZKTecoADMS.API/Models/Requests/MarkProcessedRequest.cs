namespace ZKTecoADMS.API.Models.Requests;

public class MarkProcessedRequest
{
    public List<Guid> LogIds { get; set; } = new();
}