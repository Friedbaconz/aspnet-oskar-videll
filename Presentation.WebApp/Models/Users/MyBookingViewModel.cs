using Domain.Aggregates.Bookings;
using Domain.Aggregates.Workouts;

namespace Presentation.WebApp.Models.Users
{
    public class MyBookingViewModel
    {
        public IEnumerable<Workout> Workouts { get; set; } = [];
    }
}
