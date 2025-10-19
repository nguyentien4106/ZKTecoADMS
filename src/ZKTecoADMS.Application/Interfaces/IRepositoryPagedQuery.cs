using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Repositories;
using System.Linq.Expressions;

namespace ZKTecoADMS.Application.Interfaces
{
    public interface IRepositoryPagedQuery<TEntity> : IRepository<TEntity> where TEntity : Entity<Guid>
    {
        Task<PagedResult<TEntity>> GetPagedResultAsync(
            PaginationRequest request,
            Expression<Func<TEntity, bool>>? filter = null,
            string[]? includeProperties = null,
            CancellationToken cancellationToken = default
        );
    }
}
