using Domain.Aggregates.Bookings;
using Domain.Aggregates.Workouts;
using Presentation.WebApp.Models.Bookings;
using Presentation.WebApp.Models.CostumerService;

namespace Presentation.WebApp.Models.Users
{
    public class MyBookingViewModel
    {
        public List<Workout> Workouts { get; set; } = [];

        public BookingViewModel? BookingViewModel { get; set; }

        public WorkoutViewModel? workoutViewModels { get; set; }
    }
}
