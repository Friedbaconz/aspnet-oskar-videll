namespace Application.Users;

public interface IUserRepositoryService
{
    Task<bool> UserExistsAsync(string userId, CancellationToken ct = default);
}
