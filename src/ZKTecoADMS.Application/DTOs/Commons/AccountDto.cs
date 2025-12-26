namespace ZKTecoADMS.Application.DTOs.Commons;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName {get;set;} = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string? PhoneNumber { get; set; }
    public List<string> Roles { get; set; } = [];
    public Guid? ManagerId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? ManagerName { get; set; }
    public DateTime CreatedAt { get; set; }

}
