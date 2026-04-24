using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Bookings
{
    public class RegisterBookingForm
    {

        [Required(ErrorMessage = "Workout is required")]
        public string WorkoutId { get; set; } = null!;
    }
}
