using Domain.Aggregates.Workouts;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Bookings
{
    public class RegisterBookingForm
    {
        public IEnumerable<Workout> Workouts { get; set; } = [];

        [Required(ErrorMessage = "Workout is required")]
        [Display(Name = "Workout", Prompt = "Select a workout")]
        public string WorkoutId { get; set; } = null!;

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name", Prompt = "Enter First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name", Prompt = "Enter Last Name")]
        public string LastName { get; set; } = null!;

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone Number", Prompt = "(Optional) Enter Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email", Prompt = "Enter Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Message is required.")]
        [Display(Name = "Message", Prompt = "Message..")]
        public string Message { get; set; } = null!;
    }
}
