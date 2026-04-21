using Domain.Aggregates.Bookings;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;

namespace Presentation.WebApp.Models.Bookings
{
    public class BookingViewModel
    {
        public IEnumerable<Booking> Bookings { get; set; } = [];
        public RegisterBookingForm RegisterBookingForm { get; set; } = null!;
        public UpdateBookingForm UpdateBookingForm { get; set; } = null!;
    }
}
