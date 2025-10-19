namespace ZKTecoADMS.Api.Models.Requests;

public class CreateUserRequest
{
    public string PIN { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? CardNumber { get; set; }
    public string? Password { get; set; }
    public int GroupId { get; set; } = 1;
    public int Privilege { get; set; } = 0;
    public int VerifyMode { get; set; } = 0;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Department { get; set; }
    public List<Guid> DeviceIds { get; set; }
}