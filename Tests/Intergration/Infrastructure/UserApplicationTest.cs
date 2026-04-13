using Application.Abstractions.Identity;
using Application.Common.Results;
using Application.Users.Inputs;
using Application.Users.Services;
using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.Intergration.Infrastructure
{
    public class RegisterUserAccountServiceTests
    {
        [Fact]
        public async Task ExecuteAsync_CreatesUser_And_AddsToRepository()
        {
            var email = "test@example.com";
            var password = "P@ssw0rd!";
            var identityId = Guid.NewGuid().ToString();

            var identityService = new FakeIdentityService(identityId);
            var userRepository = new InMemoryUserRepository();

            var service = new RegisterUserAccountService(identityService, userRepository);
            var input = new RegisterUserAccountInput(email, password);

            var result = await service.ExecuteAsync(input, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(identityId, result.Value);

            var stored = await userRepository.GetUserByUserIdAsync(identityId, CancellationToken.None);
            Assert.NotNull(stored);
            Assert.Equal(identityId, stored!.UserId);
        }

        private class FakeIdentityService : IidentityService
        {
            private readonly string _idToReturn;

            public FakeIdentityService(string idToReturn) => _idToReturn = idToReturn;

            public Task<Result<string?>> CreateUserInAsync(string email, string password, CancellationToken ct = default)
            {
                return Task.FromResult(Result<string?>.Ok(_idToReturn));
            }

            public Task<Result<bool>> DeleteUserAsync(string email, CancellationToken ct = default)
            {
                return Task.FromResult(Result<bool>.Ok(true));
            }

            public Task<Result<bool>> PasswordSignInAsync(string email, string password, bool rememberMe, CancellationToken ct = default)
            {
                return Task.FromResult(Result<bool>.Ok(true));
            }

            public Task SignOutAsync(CancellationToken ct = default) => Task.CompletedTask;
        }

        private class InMemoryUserRepository : IUserRepository
        {
            private readonly List<User> _store = new();

            public Task AddAsync(User model, CancellationToken ct = default)
            {
                _store.Add(model);
                return Task.CompletedTask;
            }

            public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default)
            {
                IReadOnlyList<User> list = _store.ToList();
                return Task.FromResult(list);
            }

            public Task<User?> GetByIdAsync(string id, CancellationToken ct = default)
            {
                var user = _store.FirstOrDefault(u => u.Id == id);
                return Task.FromResult(user);
            }

            public Task<bool> RemoveAsync(User model, CancellationToken ct = default)
            {
                var removed = _store.Remove(model);
                return Task.FromResult(removed);
            }

            public Task<bool> UpdateAsync(User model, CancellationToken ct = default)
            {
                var existing = _store.FirstOrDefault(u => u.Id == model.Id);
                if (existing is null) return Task.FromResult(false);
                _store.Remove(existing);
                _store.Add(model);
                return Task.FromResult(true);
            }

            public Task<User?> GetUserByUserIdAsync(string UserId, CancellationToken ct = default)
            {
                var user = _store.FirstOrDefault(u => u.UserId == UserId);
                return Task.FromResult(user);
            }

            public Task<bool> DeleteAsync(User model, CancellationToken ct = default)
            {
                var removed = _store.Remove(model);
                return Task.FromResult(removed);
            }

            public string GetUserId(User model) => model.UserId;
        }
    }
}


