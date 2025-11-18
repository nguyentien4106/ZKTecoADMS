using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ZKTecoADMS.Infrastructure.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<ApplicationUser> GetUserByUserNameAsync(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with username '{userName}' not found.");
        }
        return user;
    }

    public async Task<ApplicationUser> GetUserByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with email '{email}' not found.");
        }
        return user;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID '{userId}' not found.");
        }
        return user;
    }
}