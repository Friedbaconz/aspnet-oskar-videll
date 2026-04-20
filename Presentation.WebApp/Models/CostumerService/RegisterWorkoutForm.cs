namespace Presentation.WebApp.Models.CostumerService;

public class RegisterWorkoutForm
{
    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

}
