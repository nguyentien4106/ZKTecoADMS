using ZKTecoADMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ZKTecoADMS.Application.Interfaces;

public interface IZKTecoDbContext
{
    DbSet<ApplicationUser> Users { get; }

    DbSet<UserRefreshToken> UserRefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();
}
