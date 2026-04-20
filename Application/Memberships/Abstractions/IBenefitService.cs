using Domain.Aggregates.Memberships;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Memberships.Abstractions;

public interface IBenefitService
{
    Task<IReadOnlyList<MembershipBenefits>> GetBenefitsAsync(CancellationToken ct = default);
}
