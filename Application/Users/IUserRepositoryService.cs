namespace Application.Users;

public interface IUserRepositoryService
{
    Task<bool> UserExistsAsync(Guid userId, CancellationToken ct = default);
}
