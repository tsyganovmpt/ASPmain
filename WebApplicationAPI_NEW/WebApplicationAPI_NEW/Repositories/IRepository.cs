using System.Linq.Expressions;

namespace ApiAsp0.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Query(params string[] includePaths);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Remove(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
