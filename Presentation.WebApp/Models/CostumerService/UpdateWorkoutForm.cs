using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.CostumerService;

public class UpdateWorkoutForm
{
    public string Id { get; set; } = null!;

    [Required(ErrorMessage = "Name is required.")]
    [Display(Name = "Workout Name", Prompt = "Enter Workout Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Category is required.")]
    [Display(Name = "Workout Catergory", Prompt = "Enter Workout Catergory")]
    public string Category { get; set; } = null!;

    [Required(ErrorMessage = "Instructions are required.")]
    [Display(Name = "Workout Catergory", Prompt = "Enter Workout Instructions")]
    public string Instructions { get; set; } = null!;

    [Required(ErrorMessage = "Date is required.")]
    [Display(Name = "Workout Date", Prompt = "Enter Workout Date")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Time is required.")]
    [Display(Name = "Workout Time", Prompt = "Enter Workout Time")]
    public TimeSpan Time { get; set; }

    public List <string> Users { get; set; } = new List<string>();
}
