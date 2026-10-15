using ApiAsp0.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiAsp0.Services;

public sealed record PageResult<T>(IReadOnlyList<T> Items, int TotalCount);
public sealed record CreateResult<T>(int Id, T Item);

public interface ICrudService<TEntity, TReadDto, TCreateDto, TUpdateDto, TPatchDto>
    where TEntity : class
{
    Task<PageResult<TReadDto>> GetPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<TReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CreateResult<TReadDto>> CreateAsync(TCreateDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, TUpdateDto dto, CancellationToken ct = default);
    Task<bool> PatchAsync(int id, TPatchDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}

public sealed class CrudService<TEntity, TReadDto, TCreateDto, TUpdateDto, TPatchDto>
    : ICrudService<TEntity, TReadDto, TCreateDto, TUpdateDto, TPatchDto>
    where TEntity : class
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    private readonly string _keyProperty;
    private readonly Func<TEntity, int> _getId;
    private readonly string[] _includes;

    public CrudService(
        IRepository<TEntity> repository,
        IMapper mapper,
        string keyProperty,
        Func<TEntity, int> getId,
        params string[] includes)
    {
        _repository = repository;
        _mapper = mapper;
        _keyProperty = keyProperty;
        _getId = getId;
        _includes = includes;
    }

    public async Task<PageResult<TReadDto>> GetPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _repository.Query(_includes);
        var total = await query.CountAsync(ct);
        var entities = await query
            .OrderBy(e => EF.Property<int>(e, _keyProperty))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PageResult<TReadDto>(_mapper.Map<List<TReadDto>>(entities), total);
    }

    public async Task<TReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repository.Query(_includes)
            .FirstOrDefaultAsync(e => EF.Property<int>(e, _keyProperty) == id, ct);
        return entity is null ? default : _mapper.Map<TReadDto>(entity);
    }

    public async Task<CreateResult<TReadDto>> CreateAsync(TCreateDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.AddAsync(entity, ct);
        await _repository.SaveChangesAsync(ct);

        var id = _getId(entity);
        // Перечитываем созданную запись с навигационными свойствами для DTO чтения.
        var savedEntity = await _repository.Query(_includes)
            .FirstOrDefaultAsync(e => EF.Property<int>(e, _keyProperty) == id, ct);
        return new CreateResult<TReadDto>(id, _mapper.Map<TReadDto>(savedEntity ?? entity));
    }

    public async Task<bool> UpdateAsync(int id, TUpdateDto dto, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null) return false;

        _mapper.Map(dto, entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> PatchAsync(int id, TPatchDto dto, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null) return false;

        _mapper.Map(dto, entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null) return false;

        _repository.Remove(entity);
        await _repository.SaveChangesAsync(ct);
        return true;
    }
}
