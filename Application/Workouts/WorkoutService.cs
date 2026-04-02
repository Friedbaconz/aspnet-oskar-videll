using Application.Workouts.Services;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Workouts;

namespace Application.Workouts;

public sealed class WorkoutService(IWorkoutRepository repo) : IWorkoutService
{
    public Task<IReadOnlyList<Workout>> GetWorkoutsAsync(CancellationToken ct = default)
    {
        var workouts = repo.GetAllAsync(ct);
        return workouts;
    }
}
