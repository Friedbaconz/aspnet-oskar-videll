using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
namespace Domain.Aggregates.Bookings;

public sealed class Booking
{

    public Booking(string id, string userId, string workoutId)
    {
        Id = RequiredString(id, nameof(Id));
        UserId = RequiredString(userId, nameof(UserId));
        WorkoutId = RequiredString(workoutId, nameof(WorkoutId));
    }

    public string Id { get; private set; }

    public string UserId { get; private set; }

    public string WorkoutId { get; private set; }

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

    public static Booking Create(string id, string userId, string workoutId)
    {
        return new Booking(id, userId, workoutId);
    }

    public void Update(string id, string userId, string workoutId)
    {
        Id = RequiredString(id, nameof(Id));
        UserId = RequiredString(userId, nameof(UserId));
        WorkoutId = RequiredString(workoutId, nameof(WorkoutId));
    }
}
