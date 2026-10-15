using ApiAsp0.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAsp0.Repositories;

public sealed class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AspfisrtContext _context;
    private readonly DbSet<TEntity> _set;

    public Repository(AspfisrtContext context)
    {
        _context = context;
        _set = context.Set<TEntity>();
    }

    public IQueryable<TEntity> Query(params string[] includePaths)
    {
        IQueryable<TEntity> query = _set.AsQueryable();
        foreach (var path in includePaths.Where(p => !string.IsNullOrWhiteSpace(p)))
            query = query.Include(path);
        return query;
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _set.FindAsync(new object?[] { id }, cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
    }

    public void Remove(TEntity entity) => _set.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
