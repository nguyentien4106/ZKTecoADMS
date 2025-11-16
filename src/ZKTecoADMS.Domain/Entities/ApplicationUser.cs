using Microsoft.AspNetCore.Identity;

namespace ZKTecoADMS.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public UserRefreshToken? RefreshToken { get; set; } = null!;
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation Properties
    public virtual ICollection<Device> Devices { get; set; } = [];
    public virtual User? User { get; set; }
}
