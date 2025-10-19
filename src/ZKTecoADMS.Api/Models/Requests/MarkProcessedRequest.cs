namespace ZKTecoADMS.Api.Models.Requests;

public class MarkProcessedRequest
{
    public List<Guid> LogIds { get; set; } = new();
}