using Application.Common.Results;
using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Application.Workouts.Abstractions;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Memberships;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Workouts.Services;

public sealed class DeleteWorkoutService(IWorkoutRepository repo) : IDeleteWorkoutService
{
    public async Task<Result> ExecuteAsync(string id, CancellationToken ct = default)
    {
        var result = await repo.GetByIdAsync(id, ct);

        if (result == null)
        {
            throw new Exception("no membership by that id");
        }

        var success = await repo.RemoveAsync(result, ct);

        return success
                ? Result.Ok()
                : Result.Error("Failed to delete user profile");
    }
}
