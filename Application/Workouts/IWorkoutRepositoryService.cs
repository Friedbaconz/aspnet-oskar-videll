namespace Application.Workouts;

public interface IWorkoutRepositoryService
{
    Task<bool> WorkoutExistsAsync(Guid workoutId, CancellationToken ct = default);
}
