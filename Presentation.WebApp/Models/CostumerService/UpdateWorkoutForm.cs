namespace Presentation.WebApp.Models.CostumerService;

public class UpdateWorkoutForm
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Instructions { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public List <string> Users { get; set; } = new List<string>();
}
