
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
namespace Domain.Aggregates.Workouts;

public sealed class Booking
{

    public Booking(Guid id, Guid userId, Guid workoutId, User user, Workout workout)
    {
        Id = RequiredGuid(id, nameof(Id));
        UserId = RequiredGuid(userId, nameof(UserId));
        WorkoutId = RequiredGuid(workoutId, nameof(WorkoutId));
        User = user ?? throw new ArgumentNullException(nameof(user));
        Workout = workout ?? throw new ArgumentNullException(nameof(workout));
    }

    public Guid Id { get; }

    public Guid UserId { get; }

    public Guid WorkoutId { get; }

    public User User { get; }

    public Workout Workout { get; }

    private static Guid RequiredGuid(Guid value, string propertyName)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"{propertyName} is required.", propertyName);
        return value;
    }
    
    public static Booking Create(Guid userId, Guid workoutId, User user, Workout workout)
    {
        return new Booking(Guid.NewGuid(), userId, workoutId, user, workout);
    }

    public static Booking Rehydrate(Guid id, Guid userId, Guid workoutId, User user, Workout workout)
    {
        return new Booking(id, userId, workoutId, user, workout);
    }

    public void Update(Guid userId, Guid workoutId, User user, Workout workout)
    {

    }
}
