using Application.Common.Results;
using Application.Memberships.Inputs;
using Application.Workouts.Abstractions;
using Application.Workouts.Inputs;
using Domain.Abstractions.Repositories.Users;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;

namespace Application.Workouts.Services;

public sealed class RegisterWorkoutService(IWorkoutRepository repo) : IRegisterWorkoutService
{
    public async Task<Result<string?>> ExecuteAsync(RegisterWorkoutInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                return Result<string?>.BadRequest("Input must be provided");

            string id = Guid.NewGuid().ToString();

            var workout = Workout.Create(
                id: id,
                name: input.Name,
                category: input.Category,
                instructions: input.Instructions,
                date: input.Date,
                time: input.Time,
                Enumerable.Empty<string>()
            );

            await repo.AddAsync(workout, ct);

            return Result<string?>.Ok(workout.Id);
        }
        catch (Exception ex)
        {
            return Result<string?>.Error(ex.Message);
        }
    }
}
