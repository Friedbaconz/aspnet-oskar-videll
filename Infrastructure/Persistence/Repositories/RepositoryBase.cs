


using Microsoft.EntityFrameworkCore;
using Domain.Abstractions.Repositories;
namespace Infrastructure.Persistence.Repositories;

public abstract class RepositoryBase<TDomainModel, TId, TEntity, TDbContext>(TDbContext context) : IRepositoryBase<TDomainModel, TId> where TEntity : class where TDbContext : DbContext
{
    protected readonly TDbContext _Context = context;
    protected DbSet<TEntity> Set => _Context.Set<TEntity>();

    public abstract TId GetId(TDomainModel model);
    public abstract TEntity ToEntity(TDomainModel model);
    protected abstract TDomainModel ToDomainModel(TEntity entity);
    protected abstract void ApplyPropertyUpdates(TEntity entity, TDomainModel model);


    public virtual async Task AddAsync(TDomainModel model, CancellationToken ct = default)
    {
        try
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "The model cannot be null.");
            }

            var entity = ToEntity(model);
            await Set.AddAsync(entity, ct);
            await _Context.SaveChangesAsync(ct);

        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<bool> UpdateAsync(TDomainModel model, CancellationToken ct = default)
    {
        try
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "The model cannot be null.");
            }

            var id = GetId(model);

            var entity = await Set.FindAsync([id], ct);
            if (entity is null)
                return false;

            ApplyPropertyUpdates(entity, model);
            await _Context.SaveChangesAsync(ct);
            return true;

        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<bool> RemoveAsync(TDomainModel model, CancellationToken ct = default)
    {
        try
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "The model cannot be null.");
            }

            var id = GetId(model);

            var entity = await Set.FindAsync([id], ct);
            if (entity is null)
                return false;

            Set.Remove(entity);
            await _Context.SaveChangesAsync(ct);
            return true;

        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<TDomainModel?> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FindAsync([id], ct);
            return entity is null ? default : ToDomainModel(entity);
        }
        catch
        {
            throw;
        }
    }

    public virtual async Task<IReadOnlyList<TDomainModel>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var entites = await Set.AsNoTracking().ToListAsync(ct);
            return [.. entites.Select(ToDomainModel)];
        }
        catch
        {
            throw;
        }
    }
}
