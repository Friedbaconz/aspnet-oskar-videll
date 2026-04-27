using Domain.Aggregates.Bookings;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;

namespace Presentation.WebApp.Models.Bookings
{
    public class BookingViewModel
    {
        public IEnumerable<Booking> Bookings { get; set; } = [];
        public UpdateBookingForm UpdateBookingForm { get; set; } = null!;
    }
}
