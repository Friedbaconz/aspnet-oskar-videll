using Application.Common.Results;
using Application.Memberships.Services;
using Domain.Abstractions.Repositories.Memberships;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Memberships.Abstractions;

public sealed class DeleteMembershipService(IMembershipRepository repo, IBenefitRepository BenefitRepo) : IDeleteMembershipService
{
    public async Task<Result> ExecuteAsync(string id, CancellationToken ct = default)
    {
        var result = await repo.GetByIdAsync(id);
        if (result == null)
        {
            throw new Exception("no membership by that id");
        }

        foreach (var benefit in result.Benefits)
        {
            var membership = await BenefitRepo.GetAllAsync();

            foreach (var membershipItem in membership)
            {
                if (membershipItem.Benefit == benefit)
                {
                    BenefitRepo.RemoveAsync(membershipItem);
                }
            }
        }
        var success = await repo.RemoveAsync(result);

        return success
                ? Result.Ok()
                : Result.Error("Failed to delete user profile");
    }
}
