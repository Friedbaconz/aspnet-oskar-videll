namespace Domain.Abstractions.Repositories;

public interface IRepositoryBase<TDomainModel, TId, TEntity>
{
    Task AddAsync(TDomainModel model, CancellationToken ct = default);
    Task<IReadOnlyList<TDomainModel>> GetAllAsync(CancellationToken ct = default);
    Task<TDomainModel?> GetByIdAsync(TId id, CancellationToken ct = default);
    TId GetId(TDomainModel model);
    Task<bool> RemoveAsync(TDomainModel model, CancellationToken ct = default);
    TEntity ToEntity(TDomainModel model);
    Task<bool> UpdateAsync(TDomainModel model, CancellationToken ct = default);
}