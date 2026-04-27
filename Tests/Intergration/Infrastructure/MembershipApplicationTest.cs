using Application.Common.Results;
using Application.Memberships.Inputs;
using Application.Memberships.Services;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Memberships;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Intergration.Infrastructure
{
    public class MembershipApplicationTest
    {
        [Fact]
        public async Task ExecuteAsync_CreatesMembershipAndBenefits()
        {
            var benefitsInput = new List<RegisterBenfitsInput>
            {
                new("Gym access"),
                new("Pool access")
            };

            var input = new RegisterMemebershipInput(
                name: "Gold",
                description: "Gold membership",
                benefits: benefitsInput,
                status: "Active",
                type: "Standard",
                pricing: 49.99m,
                monthlyDuration: 12
            );

            var benefitRepo = new InMemoryBenefitRepository();
            var membershipRepo = new InMemoryMembershipRepository();

            var service = new RegisterMembershipService(benefitRepo, membershipRepo);

            var result = await service.ExecuteAsync(input, CancellationToken.None);

            Assert.True(result.Success);
            Assert.False(string.IsNullOrWhiteSpace(result.Value));

            var stored = await membershipRepo.GetByIdAsync(result.Value!, CancellationToken.None);
            Assert.NotNull(stored);
            Assert.Equal("Gold", stored!.Name);

            var storedBenefits = benefitRepo.GetAllForMembership(result.Value!);
            Assert.Equal(2, storedBenefits.Count);
            Assert.All(storedBenefits, b => Assert.Equal(result.Value, b.MembershipId));
        }

        [Fact]
        public async Task ExecuteAsync_NullInput_ReturnsBadRequest()
        {

            var benefitRepo = new InMemoryBenefitRepository();
            var membershipRepo = new InMemoryMembershipRepository();
            var service = new RegisterMembershipService(benefitRepo, membershipRepo);

            var result = await service.ExecuteAsync(null!, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal(ErrorTypes.BadRequest, result.ErrorType);
        }

        [Fact]
        public async Task ExecuteAsync_EmptyName_ReturnsBadRequest()
        {

            var input = new RegisterMemebershipInput(
                name: "   ",
                description: "desc",
                benefits: new List<RegisterBenfitsInput>(),
                status: "Active",
                type: "Basic",
                pricing: 10m,
                monthlyDuration: 1
            );

            var benefitRepo = new InMemoryBenefitRepository();
            var membershipRepo = new InMemoryMembershipRepository();
            var service = new RegisterMembershipService(benefitRepo, membershipRepo);

            var result = await service.ExecuteAsync(input, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal(ErrorTypes.BadRequest, result.ErrorType);
        }

        private class InMemoryMembershipRepository : IMembershipRepository
        {
            private readonly List<Membership> _store = new();

            public Task AddAsync(Membership model, CancellationToken ct = default)
            {
                _store.Add(model);
                return Task.CompletedTask;
            }

            public Task<IReadOnlyList<Membership>> GetAllAsync(CancellationToken ct = default)
            {
                IReadOnlyList<Membership> list = _store.ToList();
                return Task.FromResult(list);
            }

            public Task<Membership?> GetByIdAsync(string id, CancellationToken ct = default)
            {
                var m = _store.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(m);
            }

            public Task<bool> RemoveAsync(Membership model, CancellationToken ct = default)
            {
                var removed = _store.Remove(model);
                return Task.FromResult(removed);
            }

            public Task<bool> UpdateAsync(Membership model, CancellationToken ct = default)
            {
                var existing = _store.FirstOrDefault(x => x.Id == model.Id);
                if (existing is null) return Task.FromResult(false);
                _store.Remove(existing);
                _store.Add(model);
                return Task.FromResult(true);
            }
        }

        private class InMemoryBenefitRepository : IBenefitRepository
        {
            private readonly List<MembershipBenefits> _store = new();

            public Task AddAsync(MembershipBenefits model, CancellationToken ct = default)
            {
                _store.Add(model);
                return Task.CompletedTask;
            }

            public Task<IReadOnlyList<MembershipBenefits>> GetAllAsync(CancellationToken ct = default)
            {
                IReadOnlyList<MembershipBenefits> list = _store.ToList();
                return Task.FromResult(list);
            }

            public Task<MembershipBenefits?> GetByIdAsync(string id, CancellationToken ct = default)
            {
                var b = _store.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(b);
            }

            public Task<bool> RemoveAsync(MembershipBenefits model, CancellationToken ct = default)
            {
                var removed = _store.Remove(model);
                return Task.FromResult(removed);
            }

            public Task<bool> UpdateAsync(MembershipBenefits model, CancellationToken ct = default)
            {
                var existing = _store.FirstOrDefault(x => x.Id == model.Id);
                if (existing is null) return Task.FromResult(false);
                _store.Remove(existing);
                _store.Add(model);
                return Task.FromResult(true);
            }

            public List<MembershipBenefits> GetAllForMembership(string membershipId) =>
                _store.Where(b => b.MembershipId == membershipId).ToList();
        }
    }
}
