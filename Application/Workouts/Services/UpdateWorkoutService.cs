using Application.Common.Results;
using Application.Workouts.Abstractions;
using Application.Workouts.Inputs;
using Domain.Abstractions.Repositories.Users;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Workouts;

namespace Application.Workouts.Services;

public sealed class UpdateWorkoutService(IWorkoutRepository repo) : IUpdateWorkoutService
{
    public async Task<Result<Workout>> ExecuteAsync(UpdateWorkoutInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                throw new ArgumentException("input must be provided");

            var workout = await repo.GetByIdAsync(input.Id, ct);

            if (workout is null)
                return Result<Workout>.NotFound($"Workout with id '{input.Id}' was not found");

            workout.Update(input.Name, input.Category, input.Instructions, input.Date, input.Time, input.Users);
            var result = await repo.UpdateAsync(workout, ct);

            return !result
                ? Result<Workout>.NotFound($"Workout with user id '{workout.Id}' was not found")
                : Result<Workout>.Ok(workout);
        }
        catch (Exception ex)
        {
            return Result<Workout>.Error(ex.Message);
        }
    }
}