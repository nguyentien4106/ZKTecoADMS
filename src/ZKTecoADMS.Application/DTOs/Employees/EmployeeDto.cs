namespace ZKTecoADMS.Application.DTOs.Employees;

public class CurrentSalaryProfileDto
{
    public Guid Id { get; set; }
    public Guid SalaryProfileId { get; set; }
    public string ProfileName { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string RateTypeName { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public bool IsActive { get; set; }
}

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string Pin { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? CardNumber { get; set; }
    public string? Password { get; set; }
    public int Privilege { get; set; }
    public bool IsActive { get; set; }
    public string? Department { get; set; }
    public Guid DeviceId { get; set; }
    public string? DeviceName { get; set; }

    public AccountDto? ApplicationUser { get; set; }
    public CurrentSalaryProfileDto? CurrentSalaryProfile { get; set; }
}

public class AccountDto
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string? PhoneNumber { get; set; }

    public string? UserName { get; set; }

}