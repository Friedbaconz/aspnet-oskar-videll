using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
namespace Domain.Aggregates.Bookings;

public sealed class Booking
{
    public string Id { get; private set; } = null!;

    public string UserId { get; private set; } = null!;

    public string WorkoutId { get; private set; } = null!;

    private Booking()
    {
    }

    private Booking(string id)
    {
       Id = id;
    }

    public static Booking Create()
    {
        var booking = new Booking()
        {
            Id = Guid.NewGuid().ToString(),
        };

        return booking;
    }

    public static Booking Create(string id, string userId, string workoutId)
    {
        var booking = new Booking(id)
        {
            UserId = userId,
            WorkoutId = workoutId
        };

        return booking;
    }

    public void Update(string id, string userId, string workoutId)
    {
        if(string.IsNullOrWhiteSpace(id)) 
            throw new ArgumentNullException("Booking id is required");

        Id = id;
        UserId = userId;
        WorkoutId = workoutId;
    }
}
