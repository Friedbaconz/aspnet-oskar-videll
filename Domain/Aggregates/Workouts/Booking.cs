
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
namespace Domain.Aggregates.Workouts;

public sealed class Booking
{

    public Booking(int id, string userId, int workoutId, User user, Workout workout)
    {
        Id = RequiredInt(id, nameof(Id));
        UserId = RequiredString(userId, nameof(UserId));
        WorkoutId = RequiredInt(workoutId, nameof(WorkoutId));
        User = user ?? throw new ArgumentNullException(nameof(user));
        Workout = workout ?? throw new ArgumentNullException(nameof(workout));
    }

    public int Id { get; }

    public string UserId { get; }

    public int WorkoutId { get; }

    public User User { get; }

    public Workout Workout { get; }

    private static int RequiredInt(int value, string propertyName)
    {
        if (value <= 0)
            throw new ArgumentException($"{propertyName} must be a positive integer.", propertyName);
        return value;
    }
    private static string RequiredString(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{propertyName} is required.", propertyName);

        return value.Trim();
    }

    public static Booking Create(int id, string userId, int workoutId, User user, Workout workout)
    {
        return new Booking(id, userId, workoutId, user, workout);
    }

    public static Booking Rehydrate(int id, string userId, int workoutId, User user, Workout workout)
    {
        return new Booking(id, userId, workoutId, user, workout);
    }

    public void Update(int userId, int workoutId, User user, Workout workout)
    {

    }
}
