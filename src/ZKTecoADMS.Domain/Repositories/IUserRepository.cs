using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByPinAsync(string pin);
}
