using Application.Abstractions.Identity;
using Application.Common.Results;
using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Application.Users.Inputs;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Memberships.Services;

public class RegisterMembershipService(IBenefitRepository benfitrepo, IMembershipRepository repo) : IRegisterMembershipService
{
    public async Task<Result<string?>> ExecuteAsync(RegisterMemebershipInput input,CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                return Result<string?>.BadRequest("Input must be provided");

            if (string.IsNullOrWhiteSpace(input.name))
                return Result<string?>.BadRequest("Membership name is required");

            var name = input.name.Trim();
            var description = input.description?.Trim();
            var benefitlist = new List<MembershipBenefits>();
            var status = input.status;
            var type = input.type;
            var pricing = input.pricing;
            var monthlyDuration = input.monthlyDuration;
            string id = Guid.NewGuid().ToString();

            var membership = Membership.Create(
                id,
                name,
                description,
                benefitlist,
                status,
                type,
                pricing,
                monthlyDuration,
                Enumerable.Empty<string>()
            );

            await repo.AddAsync(membership, ct);

            foreach (var benfit in input.benefits)
            {
                    var newbenefits = MembershipBenefits.Create(
                        id: Guid.NewGuid().ToString(),
                        membershipId: membership.Id,
                        benefit: benfit.benefit
                    );

                    var update = benfitrepo?.AddAsync(newbenefits, ct);

                    benefitlist.Add(newbenefits);
            }

            await repo.UpdateAsync(membership, ct);

            return Result<string?>.Ok(membership.Id);
        }
        catch (Exception ex)
        {
            return Result<string?>.Error(ex.Message);
        }
    }
}
