using Application.Workouts.Services;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Workouts;

namespace Application.Workouts;

public sealed class WorkoutService(IWorkoutRepository repo) : IWorkoutService
{
    public async Task<IReadOnlyList<Workout>> GetWorkoutsAsync(CancellationToken ct = default)
    {
        var workouts = await repo.GetAllAsync(ct);
        return workouts;
    }
}
