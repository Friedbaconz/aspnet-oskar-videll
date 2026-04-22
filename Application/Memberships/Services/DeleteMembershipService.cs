using Application.Common.Results;
using Application.Memberships.Abstractions;
using Domain.Abstractions.Repositories.Memberships;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Memberships.Services;

public sealed class DeleteMembershipService(IMembershipRepository repo, IBenefitRepository BenefitRepo) : IDeleteMembershipService
{
    public async Task<Result> ExecuteAsync(string id, CancellationToken ct = default)
    {
        var result = await repo.GetByIdAsync(id);

        if (result == null)
        {
            throw new Exception("no membership by that id");
        }

        foreach(var benefitid in result.Benefits.ToList())
        {
            var benefitresult = await BenefitRepo.GetByIdAsync(benefitid.Id);

            if (benefitresult != null)
            {
                var delete = await BenefitRepo.RemoveAsync(benefitresult);
            }

        }
        
        var success = await repo.RemoveAsync(result);

        return success
                ? Result.Ok()
                : Result.Error("Failed to delete user profile");
    }

    public async Task<Result> DeleteBenefit(string id, CancellationToken ct = default)
    {
        var result = await BenefitRepo.GetByIdAsync(id);

        if (result == null)
        {
            throw new Exception("no membership by that id");
        }

        var success = await BenefitRepo.RemoveAsync(result);

        return success
            ? Result.Ok()
            : Result.Error("Failed to delete user profile");
    }
}
