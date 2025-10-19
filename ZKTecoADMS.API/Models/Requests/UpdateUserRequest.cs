namespace ZKTecoADMS.API.Models.Requests;

public class UpdateUserRequest
{
    public string? FullName { get; set; }
    public string? CardNumber { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public bool? IsActive { get; set; }
}