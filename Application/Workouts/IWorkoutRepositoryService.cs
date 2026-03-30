namespace Application.Workouts;

public interface IWorkoutRepositoryService
{
    Task<bool> WorkoutExistsAsync(int workoutId, CancellationToken ct = default);
}
