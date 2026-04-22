using Application.Memberships.Abstractions;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Memberships;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Memberships
{
    public sealed class BenefitService(IBenefitRepository repo) : IBenefitService
    {
        public async Task<IReadOnlyList<MembershipBenefits>> GetBenefitsAsync(CancellationToken ct = default)
        {
            var benefits = await repo.GetAllAsync(ct);
            return benefits;
        }

        public async Task<MembershipBenefits?> GetBenefitByIdAsync(string id, CancellationToken ct = default)
        {
            var benefit = await repo.GetByIdAsync(id, ct);
            return benefit;
        }
    }
}
