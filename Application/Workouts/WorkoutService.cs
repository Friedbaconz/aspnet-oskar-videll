using Application.Workouts.Abstractions;
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

    public async Task<Workout?> GetWorkoutByIdAsync(string id, CancellationToken ct = default)
    {
        var workout = await repo.GetByIdAsync(id, ct);

        if(workout == null)
        {
            throw new ArgumentNullException(nameof(workout));
        }

        return workout;
    }
}
