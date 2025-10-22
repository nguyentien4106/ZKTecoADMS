using Microsoft.Extensions.Logging;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Repositories;

public class UserRepository(
    ZKTecoDbContext context,
    ILogger<EfRepository<User>> logger
    ) : EfRepository<User>(context, logger), IUserRepository
{
    public async Task<User?> GetUserByPinAsync(string pin)
    {
        return await GetSingleAsync(u => u.PIN == pin );
    }
}