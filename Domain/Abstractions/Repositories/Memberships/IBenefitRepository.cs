using Domain.Aggregates.Memberships;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions.Repositories.Memberships;

public interface IBenefitRepository : IRepositoryBase<MembershipBenefits, string>
{
}
