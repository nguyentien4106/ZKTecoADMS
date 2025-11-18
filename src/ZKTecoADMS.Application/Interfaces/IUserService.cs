namespace ZKTecoADMS.Application.Interfaces;

public interface IUserService 
{
    Task<ApplicationUser> GetUserByUserNameAsync(string userName);
    Task<ApplicationUser> GetUserByEmailAsync(string email);
    Task<ApplicationUser> GetUserByIdAsync(Guid userId);
}